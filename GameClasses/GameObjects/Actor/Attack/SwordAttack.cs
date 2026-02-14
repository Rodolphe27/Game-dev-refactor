using Microsoft.Xna.Framework;

public class SwordAttack : Attack
{
    public override bool PlayerAttack { get; set; } = true;
    public override int DamageOnOthers { get; init; }
    public override bool IsProjectile {get; } = false;
    public override IHitShape GetHitShape()
    {
        return new RectHitShape(Rect);
    }
    public SwordAttack(Actor actor, int damageOnOthers)
    {
        DamageOnOthers = damageOnOthers;
        TiedActor = actor;
        Direction = TiedActor.Direction;
        AttackAnimation = new Animation(TextureLoader.TextureList[TextureListID.SwordBerrie], frameTime: 0.0015);
    }
    public override void Update(GameTime gameTime, Tile[,] map, double levelWidth)
    {
        AlignPosToActor();
        if (AttackAnimation.Update(gameTime))
            HealthPoints = -1;
    }
    public void AlignPosToActor()
    {
        float PosY = -((Height - TiedActor.Height) / 2) + TiedActor.Pos.Y;
        float PosX = Direction == Directions.Right ? TiedActor.Pos.X + TiedActor.Width : TiedActor.Pos.X - Width;
        Pos = new Vector2(PosX, PosY);
    }
}