
using Microsoft.Xna.Framework;
using System;

public class FireballAttack : Attack
{
    public override bool PlayerAttack { get; set; } = true;
    public override bool IsProjectile {get; } = true;
    private float SpeedX;
    private float SpeedY;
    private int Dir;
    private Capsules Capsule;
    public override Rectangle Rect => new((int)Pos.X - Width / 2, (int)Pos.Y - Height / 2, Width, Height);
    public override int DamageOnOthers { get; init; }
    public override IHitShape GetHitShape()
    {
        return new CapsuleHitShape(Capsule);
    }
    public FireballAttack(Actor actor, float rotationDeg, int damageOnOthers, int posOffet = 0)
    {
        DamageOnOthers = damageOnOthers; 
        TiedActor = actor; // The player who launched the fireball
        Direction = TiedActor.Direction; 
        AttackAnimation = new Animation(TextureLoader.TextureList[TextureListID.FireballVanko], frameTime: 0.20);

        Dir = Direction == Directions.Right ? 1 : -1;

        // Speed on the X axis flips based on the direction
        SpeedX = (float)Math.Cos(MathHelper.ToRadians(rotationDeg)) * 3f * Dir;
        SpeedY = (float)Math.Sin(MathHelper.ToRadians(rotationDeg)) * 0.3f;

        // Starting position of the fireball, centered on the actor
        float PosY = TiedActor.Pos.Y + TiedActor.Height / 2;
        float PosX = TiedActor.Pos.X + TiedActor.Width / 2;

        // Center position
        Pos = new Vector2(PosX + Width / 2, PosY + Height / 2 + posOffet);
    }
    public override void Update(GameTime gameTime, Tile[,] map, double levelWidth)
    {
        if (AttackAnimation.Update(gameTime))
            HealthPoints = -1;

        Pos = new Vector2(Pos.X + SpeedX, Pos.Y + SpeedY);

        Vector2 direction = new(MathF.Cos(Rotation), MathF.Sin(Rotation));
        Vector2 start = Pos + direction * Height / 2f;
        Vector2 end = Pos - direction * Height / 2f;
        Capsule = new Capsules(start, end, Height / 2);

        foreach(Tile tile in GetNearbySolidTiles(map))
        {
            if(Capsule.IntersectsRectangle(tile.Rect))
            {
                HealthPoints = -1;
            }
        }

        // TODO: Fix the code so the fireball collission with tiles looks as intended so the fireball *always* immediately vanishes
        // in the moment its capsules hits a tile, independent of the direction from which it hits the tile and rotiation.
        // I am very sure this can be archieved without changing the origin based on direction. There must be a smooth, universal
        // way to reliably display collisions based on rotation, position, Width, Height and an always centered origin.

        // Note: The way collisions between capsules and rectangles work in the capsules class is 100% working. The math might be
        // too simplified for special cases but this is confirmed to NOT cause the issues described above.
    }
}