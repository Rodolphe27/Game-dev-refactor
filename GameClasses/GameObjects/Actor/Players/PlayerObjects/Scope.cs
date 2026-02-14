
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Scope : PlayerObject
{
    private float DistanceFromPlayer = 225f;
    private int dir;
    public Scope(Actor tiedActor)
    {
        ObjectAnimation = new(TextureLoader.TextureList[TextureListID.Scope], 1);
        dir = tiedActor.Direction == Actor.Directions.Right ? 1 : -1;
        TiedActor = tiedActor;
        Origin = new Vector2(Pos.X + Texture.Width / 2, Pos.Y + Texture.Height / 2);
        Update(0);
    }
    public void Update(float rotation)
    {
        Pos = new Vector2(
            TiedActor.Pos.X + TiedActor.Width / 2 + dir * DistanceFromPlayer * (float)Math.Cos(MathHelper.ToRadians(rotation)),  // X position
            TiedActor.Pos.Y + TiedActor.Height / 2 + DistanceFromPlayer * (float)Math.Sin(MathHelper.ToRadians(rotation))   // Y position
        );
    }
}