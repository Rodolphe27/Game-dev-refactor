using Microsoft.Xna.Framework;
using System;

public class GunshotAttack : Attack
{
    public override bool PlayerAttack { get; set; } = true;
    public override bool IsProjectile {get; } = true;
    private float SpeedX;
    private float SpeedY;
    private int Dir;
    private Capsules Capsule;
    public override int DamageOnOthers { get; init; }
    public override Rectangle Rect => new((int)Pos.X - Width / 2, (int)Pos.Y - Height / 2, Width, Height);
    public override IHitShape GetHitShape()
    {
        return new CapsuleHitShape(Capsule);
    }
    public GunshotAttack(Actor actor, float rotationDeg, int damageOnOthers, int posOffet = 0)
    {
        DamageOnOthers = damageOnOthers;
        TiedActor = actor;
        Direction = TiedActor.Direction;
        AttackAnimation = new Animation(TextureLoader.TextureList[TextureListID.GunshotRoman], frameTime: 0.10);

        Dir = Direction == Directions.Right ? 1 : -1;

        SpeedX = (float)Math.Cos(MathHelper.ToRadians(rotationDeg)) * 12f * Dir;
        SpeedY = (float)Math.Sin(MathHelper.ToRadians(rotationDeg)) * 12f;

        float PosY = TiedActor.Pos.Y + TiedActor.Height / 2;
        float PosX = TiedActor.Pos.X + TiedActor.Width / 2;

        Pos = new Vector2(PosX, PosY + posOffet);
        Origin = Dir == 1 ? new Vector2(Width / 2, Height / 2) : new Vector2(Width, Height / 2);
        new SetTimer(3000).Elapsed += () => {HealthPoints = -1;};
    }
    public override void Update(GameTime gameTime, Tile[,] map, double levelWidth)
    {
        SpeedY += Globals.Gravity * 0.15f;
        Rotation = MathF.Atan2(SpeedY * Dir, SpeedX * Dir);
        Pos = new Vector2(Pos.X + SpeedX, Pos.Y + SpeedY);

        Vector2 direction = new(MathF.Cos(Rotation), MathF.Sin(Rotation));
        Vector2 start = Pos + direction * Height / 2f;
        Vector2 end = Pos - direction * Height / 2f;
        Capsule = new Capsules(start, end, Height / 2);

        foreach(Tile tile in GetNearbySolidTiles(map))
        {
            if(Capsule.IntersectsRectangle(tile.Rect))
                HealthPoints = -1;
        }
        AttackAnimation.Update(gameTime);
    }
}