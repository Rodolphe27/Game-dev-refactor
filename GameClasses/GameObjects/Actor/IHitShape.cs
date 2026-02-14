using Microsoft.Xna.Framework;

public interface IHitShape
{
    bool Intersects(Rectangle rect);
}