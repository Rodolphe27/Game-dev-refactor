using System;
using System.Numerics;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

public class GhostFireballAttack : Attack
{
    public override bool IsProjectile { get; } = true;

    public override bool PlayerAttack { get; set; } = false;
    public override int DamageOnOthers { get; init; }

    private Capsules capsules;
    private float speed = 4.0f;
    private Vector2 velocity;

    public GhostFireballAttack(Actor ghost, float rotationDeg, int damage)
    {
        DamageOnOthers = damage;
        TiedActor = ghost;
        AttackAnimation = new Animation(TextureLoader.TextureList[TextureListID.FireballVanko], frameTime: 0.20);

        float startX = ghost.Pos.X + ghost.Width /2;
        float startY = ghost.Pos.Y + ghost.Height /2;
        Pos = new Vector2(startX, startY);

        Rotation = MathHelper.ToRadians(rotationDeg);

        velocity = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation))* speed;

        


    }


    public override IHitShape GetHitShape()
    {
        return new CapsuleHitShape(capsules);
    }

    public override void Update(GameTime gameTime, Tile[,] map, double levelWidth)
    {
        if(AttackAnimation.Update(gameTime)) {}

        Pos += velocity;

        Origin = new Vector2(Width/2f, Height /2f);
        float capsulesRadius = Height /2f;
        float capsulesOffset = (Width/ 2f) - capsulesRadius;
        if(capsulesOffset <0) capsulesOffset = 0;

        Vector2 dirVec = new (MathF.Cos(Rotation), MathF.Sin(Rotation));
        Vector2 start = Pos + dirVec * capsulesOffset;
        Vector2 end = Pos - dirVec * capsulesOffset;

        capsules = new Capsules(start, end, capsulesRadius);

        foreach(Tile tile in GetNearbySolidTiles(map))
        {
            if(capsules.IntersectsRectangle(tile.Rect))
            {
                HealthPoints = -1;

            }
        }

    }
}