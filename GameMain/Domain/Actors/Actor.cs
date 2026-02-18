using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class Actor : GameObject
{
    public event Action OnDestroyed;
    public enum Directions { Left, Right, }
    public enum MovStates { Default, Moving, Jumping, Attacking}
    public Directions Direction = Directions.Right;
    public MovStates CurrState = MovStates.Default;
    public MovStates PrevState = MovStates.Default;
    public Vector2 Origin = Vector2.Zero;
    public float Rotation = 0f;
    public IActorState ActorState { get; set; }
    public bool HasReceivedDamage = false;
    public virtual int HealthPoints { get; set; } = 1;
    public abstract int DamageOnOthers { get; init; }
    public override Texture2D Texture => ActorState.Animations[CurrState].GetCurrentFrame();
    public void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
    public void UpdateAnimation(GameTime gameTime)
    {
        if (CurrState != PrevState)
            ActorState.Animations[CurrState].Reset();

        ActorState.Animations[CurrState].Update(gameTime);
        PrevState = CurrState;
        Height = Texture.Height;
        Width = Texture.Width;
    }
    public int HandleLeft(List<Tile> tiles)
    {
        Direction = Directions.Left;
        
        // NoClip: Ignoriere Kollisionen und erhöhe Geschwindigkeit
        if (NoClip.IsEnabled)
        {
            float noClipSpeed = ActorState.MoveSpeed * 2.5f; // 2.5x schneller
            Pos = new Vector2(Pos.X - noClipSpeed, Pos.Y);
            return -1;
        }
        float speed = ActorState.MoveSpeed;

        if (this is Players player && player.IsUsingWings)
        {
            speed += 1.5f;
        }
        Rectangle nextPos = new((int)(Pos.X - speed), (int)Pos.Y, Width, Height);
        if (nextPos.X < 0) return 0;
        foreach (var tile in tiles)
        {
            if (nextPos.Intersects(tile.Rect)) return 0;
        }
        Pos = new Vector2(Pos.X - speed, Pos.Y);
        return -1;
    }
    public int HandleRight(List<Tile> tiles, double levelLength)
    {
        Direction = Directions.Right;
        
        // NoClip: Ignoriere Kollisionen und erhöhe Geschwindigkeit
        if (NoClip.IsEnabled)
        {
            float noClipSpeed = ActorState.MoveSpeed * 2.5f; // 2.5x schneller
            Pos = new Vector2(Pos.X + noClipSpeed, Pos.Y);
            return -1;
        }
        float speed = ActorState.MoveSpeed;

        if (this is Players player && player.IsUsingWings)
        {
            speed += 1.5f;
        }
        
        Rectangle nextPos = new((int)(Pos.X + speed), (int)Pos.Y, Width, Height);
        if (nextPos.X + Width - 1 > levelLength) return 0;
        foreach (var tile in tiles)
        {
            if (nextPos.Intersects(tile.Rect)) return 0;
        }
        Pos = new Vector2(Pos.X + speed, Pos.Y);
        return -1;
    }
    public abstract void Update(GameTime gameTime, Tile[,] map, double levelWidth);
    public override void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effect = Direction == Directions.Left 
        ? SpriteEffects.None 
        : SpriteEffects.FlipHorizontally;

        Color tint = Color.White;

        if (HasReceivedDamage)
            tint = ((int)(Globals.TotalTime / 100) % 2) == 0 ? Color.White : Color.White * 0f;
            
        spriteBatch.Draw(Texture, Pos, null, tint, Rotation, Origin, 1f, effect, 0f);
    }
}