
using System;
using Microsoft.Xna.Framework;

public class Capsules(Vector2 start, Vector2 end, float radius)
{
    public Vector2 Start = start;
    public Vector2 End = end;
    public float Radius = radius;
    public bool IntersectsRectangle(Rectangle rect)
    {
        foreach (Vector2 point in new Vector2[] { Start, End })
        {
            float X = Math.Clamp(point.X, rect.Left, rect.Right - 1);
            float Y = Math.Clamp(point.Y, rect.Top, rect.Bottom - 1);
            Vector2 ClosetPointOnRect = new(X, Y);

            if(Vector2.DistanceSquared(ClosetPointOnRect, point) <= (Radius * Radius))
                return true;
        }
        return false;
    }
}