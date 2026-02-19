
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class Enemy : Actor
{
    public event Action<Attack[]> OnAttack;

    public abstract Items OnDeath(Players player);

    public void SpawnAttack(Attack[] attacks)
    {
        OnAttack?.Invoke(attacks);

    }
    public override float MaxFallSpeed => ActorState.MaxFallSpeed;
    public abstract int XPdrop { get; }
    public virtual float SightRange { get; protected set; } = 300f;
   
    public IEnemyState EnemyState { get; protected set; }
    public override int HealthPoints
    {
        get => ActorState.HealthPoints;
        set => ActorState.HealthPoints = value;
    }
    public bool IsGrounded { get; private set; } = false;
   

    //healtbar

    private float healthBarTimer = 0f;
    private const float HealthBarTimerDuration = 2.0f;

    public void ReceiveDamage(int damage)
    {
        if(!HasReceivedDamage) {
            HealthPoints -= damage;

            healthBarTimer = HealthBarTimerDuration;
            HasReceivedDamage = true;
            new SetTimer(450).Elapsed += () => {HasReceivedDamage = false;};
            AudioLoader.Audio[AudioID.DamageTaken].Play();
        }
    }
    public void SetState(IEnemyState newState)
    {
        EnemyState?.Exit(this);
        EnemyState = newState;
        EnemyState.Enter(this);
        

    }


    public override void Update(GameTime gameTime, Tile[,] map, double levelWidth)
    {

        var player = Globals.players;
        EnemyState?.Update(this, player, map, levelWidth);
        UpdateAnimation(gameTime);

        if (healthBarTimer > 0)
        {
            healthBarTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        
       
       

    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        // 1. Draw Enemy Normal (No Red Flash)
        base.Draw(spriteBatch);

        // 2. Draw Health Bar (Only if timer is running and enemy is alive)
        if (healthBarTimer > 0 && HealthPoints > 0)
        {
            DrawHealthBar(spriteBatch);
        }
    }

    private void DrawHealthBar(SpriteBatch spriteBatch)
    {
        // Settings
        int height = 5;     // Height of the bar
        int yOffset = -15;  // Pixels above the enemy head
        int pixelsPerHP = 2; // How wide is 1 HP? (20 HP = 40 pixels)

        // Calculate the current width based solely on current HealthPoints
        int currentWidth = HealthPoints * pixelsPerHP;

        // Calculate Position (Centered horizontally based on the variable width)
        Vector2 barPos = new Vector2(Pos.X - currentWidth / 2, Pos.Y - (Height / 2) + yOffset);

        // Draw ONLY the Green Bar (Foreground)
        // We cannot draw a red background "container" because we don't know the MaxHealth.
        Rectangle fgRect = new Rectangle((int)barPos.X, (int)barPos.Y, currentWidth, height);
        
        // Ensure TextureLoader.Pixel exists (or use a 1x1 white texture)
        spriteBatch.Draw(TextureLoader.Pixel, fgRect, Color.LimeGreen);
    }

   public void ApplyGravity(Tile[,] map)
    {
        // Wir nutzen deine existierende UpdateFalling Logik
        List<Tile> nearbyTiles = GetNearbySolidTiles(map, 1);
        int fallResult = UpdateFalling(nearbyTiles);

        if (fallResult == 1) // 1 bedeutet: Boden berührt
        {
            IsGrounded = true;
            // Wenn wir gerade "Jumping" waren, sind wir jetzt gelandet
            if (CurrState == MovStates.Jumping) CurrState = MovStates.Moving;
        }
        else if (fallResult == -1) // -1 bedeutet: Wir fallen/fliegen
        {
            IsGrounded = false;
            CurrState = MovStates.Jumping;
        }
        // fallResult 0 wäre Decke, ignorieren wir hier erstmal
    }

    public void Jump()

    {
        // Optional: Nur springen, wenn am Boden. 
        // Für "Emergency Jumps" (aus Lava raus) kann man das aber weglassen.
        // if (!IsGrounded) return; 

        // Hole Sprungkraft aus den Stats oder nutze Standardwert
        float jumpForce = ActorState != null ? ActorState.JumpSpeed : -9.0f;
        
        // MoveSpeed ist in GameObject deine Y-Velocity (Vertikal)
        MoveSpeed = jumpForce; 
        
        IsGrounded = false;
        CurrState = MovStates.Jumping;
    }

    public bool CanSeePlayer(Players player)
    {
        if (player == null) return false;
        float dist = Vector2.Distance(this.Pos, player.Pos);
        return dist < this.SightRange;
    }

    internal void FLyTowards(Vector2 targetPos)
    {
        Vector2 direction = targetPos - this.Pos;

    if (direction != Vector2.Zero)
    {
        direction.Normalize();
    }

    float speed = ActorState != null ? ActorState.MoveSpeed : 1.5f;

    Pos += direction * speed;

    if (direction.X > 0) Direction = Directions.Left;
    else if (direction.X < 0) Direction = Directions.Right;

    CurrState = MovStates.Moving;
    }

   
        

    
}