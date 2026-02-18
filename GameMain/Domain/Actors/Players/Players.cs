
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

public class Players : Actor
{
    public enum PlayerState
    {
        Berrie, Roman, Vanko
    }
    public override float MaxFallSpeed => ActorState.MaxFallSpeed;
    public override int HealthPoints
    {
        get => ActorState.HealthPoints;
        set => ActorState.HealthPoints = value;
    }
    public override int DamageOnOthers { get; init; } = 0;
    private bool CanJump = true;
    private bool CanJumpBoost = false;
    private int FramesSinceJumped = 0;
    public event Action<Attack[]> CreateAttack;
    public event Action OnPlayerDeath;
    public event Action OnWin;
    private readonly List<IPlayerState> allStates = [];
    private int stateIndex = 0;
    private List<PlayerObject> PlayerObjects = [];
    private double RegeneratingTimer = 0;
    public bool IsUsingWings = false;
    public int NumberHealthPotions = 0;
    public int NumberKeySilver = 0;
    public int NumberKeyGold = 0;
    public int NumberWings = 0;
    public int KilledWitches = 0;

    public Players()
    {
        allStates.Add(new BerrieState());
        allStates.Add(new RomanState());
        allStates.Add(new VankoState());
        ActorState = allStates[0];
    }
    public void NextCharacter()
    {
        if(ActorState.HealthPoints <= 0)
        {
            allStates.RemoveAt(stateIndex);
            if(allStates.Count == 0)
            {
                OnPlayerDeath?.Invoke();
                AudioLoader.Audio[AudioID.GameOver].Play();
                return;
            }
        }
        stateIndex = (stateIndex + 1) % allStates.Count;
        ActorState = allStates[stateIndex];
        CurrState = MovStates.Default;
        PlayerObjects.RemoveAll(obj => obj is Scope);
    }
    public IPlayerState GetCurrentPlayerState()
    {
        return allStates[stateIndex];
    }
    
    /// <summary>
    /// Prüft ob Spieler genug Schlüssel hat um Portal zu benutzen (1 erforderlich) oder genug Gegner besiegt wurden (10 erforderlich)
    /// </summary>
    public bool CanUsePortal()
    {
        return NumberKeyGold >= 1 && Globals.EnemyDiedCounter >= 0;
    }
    public void ReceiveDamage(int damage)
    {
        if(!HasReceivedDamage) {
            HealthPoints -= damage;
            if (HealthPoints <= 0) NextCharacter();
            HasReceivedDamage = true;
            new SetTimer(1500).Elapsed += () => {HasReceivedDamage = false;};
            AudioLoader.Audio[AudioID.DamageTaken].Play();
        }
    }
    private void HandleUpDown()
    {
        // NoClip: Freie vertikale Bewegung mit erhöhter Geschwindigkeit
        if (NoClip.IsEnabled)
        {
            float noClipSpeed = ActorState.MoveSpeed * 2.5f; // 2.5x schneller im NoClip
            if (Controls.PressUp)
            {
                Pos = new Vector2(Pos.X, Pos.Y - noClipSpeed);
            }
            if (Controls.PressDown)
            {
                Pos = new Vector2(Pos.X, Pos.Y + noClipSpeed);
            }
            return;
        }

        if (Controls.NewPressUp)
        {
            if (CanJump && !(CurrState == MovStates.Attacking && ActorState is not RomanState))
            {
                int multiplicator = IsUsingWings ? 2 : 1;
                MoveSpeed = ActorState.JumpSpeed * multiplicator;
                CanJump = false;
                CanJumpBoost = true;
                FramesSinceJumped = 0;
                AudioLoader.Audio[AudioID.Jump].Play();
            }
        }
        if (Controls.PressUp && CanJumpBoost && FramesSinceJumped < 18)
        {
            MoveSpeed += ActorState.JumpBoost;
        }
        else if (Controls.PressDown)
        {
            MoveSpeed += 0.3f;
            CanJumpBoost = false;
        }
        else
        {
            CanJumpBoost = false;
        }
        FramesSinceJumped = Math.Min(FramesSinceJumped + 1, 18);
    }
    private void HandleStateAttack()
    {

        if(ActorState is IPlayerState state && state.Energy > state.AttackDamage) {

            state.Energy -= state.AttackDamage;
            
            CurrState = MovStates.Attacking;

            if(ActorState is BerrieState)
            {
                Attack[] attacks = ActorState.Attack(this, 0f);
                CreateAttack?.Invoke(attacks);
                new SetTimer(285).Elapsed += () => {CurrState = MovStates.Default;};
                AudioLoader.Audio[AudioID.Sword].Play();
            }
            else if(ActorState is VankoState Vstate)
            {
                Attack[] attacks = ActorState.Attack(this, 0f);
                CreateAttack?.Invoke(attacks);
                new SetTimer(500).Elapsed += () => {CurrState = MovStates.Default;};
                AudioLoader.Audio[AudioID.Fireball].Play();
            }
            else if(ActorState is RomanState Rstate)
            {
                Rstate.ShootReady = false;
                PlayerObjects.Add(new Scope(this));
                new SetTimer(600).Elapsed += () => {Rstate.ShootReady = true;};
            }
        }
    }
    public void UpdateStates()
    {
        // Check for XP
        if(GetCurrentPlayerState().XP >= GetCurrentPlayerState().XPMax)
        {
            if(GetCurrentPlayerState().LV < 2)
            {
                GetCurrentPlayerState().LV += 1;
                GetCurrentPlayerState().XP -= GetCurrentPlayerState().XPMax;
                GetCurrentPlayerState().Energy = GetCurrentPlayerState().EnergyMax;
                GetCurrentPlayerState().HealthPoints = GetCurrentPlayerState().HPMax;
                AudioLoader.Audio[AudioID.LevelUp].Play();
            }
        }
        // Update Attack behaviour
        if(ActorState is RomanState Rstate && CurrState == MovStates.Attacking)
        {
            if(Controls.PressRight) 
                Rstate.AttackRotation = Math.Min(Rstate.AttackRotation + 1f, 45f);
            if(Controls.PressLeft) 
                Rstate.AttackRotation = Math.Max(Rstate.AttackRotation - 1f, -45f);
            
            foreach(Scope scope in PlayerObjects.OfType<Scope>())
            {
                scope.Update(Rstate.AttackRotation);
            }
            if(Controls.PressSwitch)
            {
                Direction = Direction == Directions.Left ? Directions.Right : Directions.Left;
                PlayerObjects.RemoveAll(obj => obj is Scope);
                PlayerObjects.Add(new Scope(this));
            }
            if (Rstate.ShootReady && Controls.NewPressAttack)
            {
                Attack[] attacks = ActorState.Attack(this, Rstate.AttackRotation);
                CreateAttack?.Invoke(attacks);
                Rstate.AttackRotation = 0f;
                CurrState = MovStates.Default;
                PlayerObjects.RemoveAll(obj => obj is Scope);
                AudioLoader.Audio[AudioID.Shoot].Play();
            }
        }
    }
    public void RegenerateCharacters(GameTime gameTime)
    {
        RegeneratingTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

        if(RegeneratingTimer > 1000)
        {
            RegeneratingTimer = 0;
            foreach(IPlayerState state in allStates)
            {
                if(state.HealthPoints < state.HPMax)
                {
                    state.HealthPoints = Math.Min(state.HealthPoints + 1, state.HPMax);
                }
                if(state.Energy < state.EnergyMax)
                {
                    state.Energy = Math.Min(state.Energy + 2, state.EnergyMax);
                }
            }
        }
    }

    #region Initialise
    public void Initialise()
    {
        Width = Texture.Width;
        Height = Texture.Height;
        Pos = new Vector2(4 * Globals.TileSize, 26 * Globals.TileSize - Height);
    }

    #endregion

    #region Update
    public override void Update(GameTime gameTime, Tile[,] map, double levelWidth)
    {
        if(KilledWitches >= 3) OnWin?.Invoke();

        if(Globals.Time <= 0) OnPlayerDeath?.Invoke();
        
        // NoClip Toggle prüfen
        NoClip.UpdateNoClipToggle();

        Controls.Input.Update();
        List<Tile> nearbyTiles = GetNearbySolidTiles(map, 1);

        bool PlayerIsFalling = true;
        if (!NoClip.IsEnabled)
        {
            switch (UpdateFalling(nearbyTiles))
            {
                case -1:
                    CanJump = false;
                    break;
                case 0:
                    FramesSinceJumped = 18;
                    break;
                case 1:
                    PlayerIsFalling = false;
                    CanJump = true;
                    break;
            }
        }
        else
        {
            // Im NoClip: Spieler bleibt in der Luft, kann frei bewegt werden
            PlayerIsFalling = false;
            CanJump = true;
            MoveSpeed = 0;
        }

        bool PlayerMovesRightOrLeft = false;
        if(!(CurrState == MovStates.Attacking && !PlayerIsFalling) && !(CurrState == MovStates.Attacking && ActorState.Name != IActorState.Character.Berrie))
        {
            if (Controls.PressLeft && !(CurrState == MovStates.Attacking && PlayerIsFalling && Direction == Directions.Right))
            {
                HandleLeft(nearbyTiles);
                PlayerMovesRightOrLeft = true;
            }
            else if (Controls.PressRight && !(CurrState == MovStates.Attacking && PlayerIsFalling && Direction == Directions.Left))
            {
                HandleRight(nearbyTiles, levelWidth);
                PlayerMovesRightOrLeft = true;
            }   
        }

        if      (CurrState == MovStates.Attacking) {}
        else if (PlayerIsFalling)         CurrState = MovStates.Jumping;
        else if (PlayerMovesRightOrLeft)  CurrState = MovStates.Moving;
        else                              CurrState = MovStates.Default;

        HandleUpDown();

        if (Controls.NewPressCharacterSwitch && CurrState != MovStates.Attacking)
            NextCharacter();

        if(Controls.NewPressAttack)
        {
            if (CurrState != MovStates.Attacking)
            {
                HandleStateAttack();
            }
        }
        if(Controls.PressDrinkHP)
        {
            IPlayerState state = GetCurrentPlayerState();
            if(NumberHealthPotions > 0 && (state.HealthPoints < state.HPMax || state.Energy < state.EnergyMax))
            {
                NumberHealthPotions--;
                state.HealthPoints = Math.Min(state.HealthPoints + 30, state.HPMax);
                state.Energy = state.EnergyMax;
                AudioLoader.Audio[AudioID.Drink].Play();
            }
        }
        if(Controls.PressUseWings)
        {
            if(NumberWings > 0 && IsUsingWings == false)
            {
                IPlayerState state = GetCurrentPlayerState();
                NumberWings--;
                IsUsingWings = true;
                new SetTimer(10000).Elapsed += () => IsUsingWings = false;
                AudioLoader.Audio[AudioID.Wings].Play();
            }
        }
        UpdateAnimation(gameTime);
        UpdateStates();
        RegenerateCharacters(gameTime);
    }

    #endregion

    #region Draw
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        foreach(PlayerObject obj in PlayerObjects)
        {
            obj.Draw(spriteBatch);
        }
    }

    #endregion
}
