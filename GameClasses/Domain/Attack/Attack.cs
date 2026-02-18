using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class Attack : Actor
{
    public Actor TiedActor { get; set; }
    public Animation AttackAnimation;
    public abstract bool IsProjectile { get; }
    public abstract bool PlayerAttack { get; set; }
    public abstract IHitShape GetHitShape();
    public override Texture2D Texture => AttackAnimation.GetCurrentFrame();
    public override int Height => Texture.Height;
    public override int Width => Texture.Width;
}