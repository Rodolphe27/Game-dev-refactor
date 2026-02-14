using Microsoft.Xna.Framework;

public class RectHitShape(Rectangle rect) : IHitShape
{
    public Rectangle Rect = rect;
    public bool Intersects(Rectangle rect)
    {
        return Rect.Intersects(rect);
    }
}