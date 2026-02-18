using System;
using System.Numerics;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

public class WitchAttack : Attack
{
    public override bool IsProjectile { get; } = true;

    public override bool PlayerAttack { get; set; } = false;
    public override int DamageOnOthers { get; init; }

    private Capsules capsules;
    private float speed = 6.0f; // Slightly faster for a throw
    private Vector2 velocity;

    public WitchAttack(Actor witch, float rotationDeg, int damage)
    {
        DamageOnOthers = damage;
        TiedActor = witch;
        
        // Ensure you have PotionPurple in your TextureListID, otherwise use a fallback

      AttackAnimation = new Animation(TextureLoader.TextureList[TextureListID.GunshotRoman], frameTime: 0.10);

        // Calculate Start Position (Same "Center" logic as Ghost)
        float startX = witch.Pos.X + witch.Width / 2;
        float startY = witch.Pos.Y + witch.Height / 2;
        Pos = new Vector2(startX, startY);

        Rotation = MathHelper.ToRadians(rotationDeg);

        velocity = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * speed;
        
        // // Optional: Give it a slight upward toss initially so it arcs better
        // velocity.Y -= 2.0f; 
        
        // // Safety: Prevent instant deletion if your base class defaults HP to 0
        // HealthPoints = 1; 
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