
using Microsoft.Xna.Framework;

public class CapsuleHitShape(Capsules capsule) : IHitShape
{
    public Capsules Capsule = capsule;
    public bool Intersects(Rectangle rect)
    {
        return Capsule.IntersectsRectangle(rect);
    }
}