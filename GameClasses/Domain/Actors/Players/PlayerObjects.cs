
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class PlayerObject : GameObject
{
    public Actor TiedActor { get; set; }
    public Animation ObjectAnimation;
    public override Texture2D Texture => ObjectAnimation.GetCurrentFrame();
    public override int Height => Texture.Height;
    public override int Width => Texture.Width;
    public Actor.Directions Direction;
    public float Rotation = 0f;
    public Vector2 Origin = Vector2.Zero;
    public override void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effect = Direction == Actor.Directions.Left 
        ? SpriteEffects.None 
        : SpriteEffects.FlipHorizontally;

        spriteBatch.Draw(Texture, Pos, null, Color.White, 0f, Origin, 1f, effect, 0f);
    }
}