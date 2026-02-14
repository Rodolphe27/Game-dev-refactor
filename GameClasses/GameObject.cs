
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class GameObject
{
    public virtual float MoveSpeed { get; set; }
    public virtual float MaxFallSpeed { get; set; }
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
    public Vector2 Pos { get; set; } = Vector2.Zero;
    public virtual Texture2D Texture { get; set; }
    public virtual Rectangle Rect => new((int)Pos.X, (int)Pos.Y, Width, Height);

    public List<Tile> GetNearbySolidTiles(Tile[,] map, int radius = 0)
    {
        return [.. GetNearbyTiles(map, radius).Where(tile => tile.IsSolid)];
    }
    public List<Tile> GetNearbyTiles(Tile[,] map, int radius = 0)
    {
        List<Tile> nearbyTiles = [];

        int xMinTemp = (int)(Pos.X / Globals.TileSize) - radius;
        int xMaxTemp = (int)((Pos.X + Width - 1) / Globals.TileSize) + radius;
        int yMinTemp = (int)(Pos.Y / Globals.TileSize) - radius;
        int yMaxTemp = (int)((Pos.Y + Height - 1) / Globals.TileSize) + radius;

        int xMin = xMinTemp < 0 ? 0 : xMinTemp;
        int xMax = xMaxTemp >= map.GetLength(1) - 1 ? map.GetLength(1) - 1 : xMaxTemp;
        int yMin = yMinTemp < 0 ? 0 : yMinTemp;
        int yMax = yMaxTemp >= map.GetLength(0) - 1 ? map.GetLength(0) - 1 : yMaxTemp;

        for (int i = yMin; i <= yMax; i++)
        {
            for (int j = xMin; j <= xMax; j++)
            {
                nearbyTiles.Add(map[i, j]);
            }
        }
        return nearbyTiles;
    }
    public Tile GetMostOverlappingTile(Tile[,] map)
    {
        List<Tile> toCheck = GetNearbySolidTiles(map);
        double maxOverlap = -1;
        Tile mostOverlappingTile = toCheck.Count == 0 ? new() : toCheck[0];
        foreach (Tile tile in toCheck)
        {
            double left = Math.Max(Pos.X, tile.Rect.Left);
            double right = Math.Min(Pos.X + Width, tile.Rect.Right);
            double bottom = Math.Min(Pos.Y + Height, tile.Rect.Bottom);
            double top = Math.Max(Pos.Y, tile.Rect.Top);

            double height = bottom - top;
            double width = right - left;

            double overlap = height * width;
            if (overlap > maxOverlap)
            {
                maxOverlap = overlap;
                mostOverlappingTile = tile;
            }
        }
        return mostOverlappingTile;
    }
    public bool IntersectPixels(GameObject obj)
    {
        if (!Rect.Intersects(obj.Rect))
        {
            return false;
        }
        Color[] dataA = new Color[Width * Height];
        Texture.GetData(dataA);

        Color[] dataB = new Color[obj.Width * obj.Height];
        obj.Texture.GetData(dataB);

        // Find the bounds of the rectangle intersection
        int top = Math.Max(Rect.Top, obj.Rect.Top);
        int bottom = Math.Min(Rect.Bottom, obj.Rect.Bottom);
        int left = Math.Max(Rect.Left, obj.Rect.Left);
        int right = Math.Min(Rect.Right, obj.Rect.Right);

        // Check every point within the intersection bounds
        for (int y = top; y < bottom; y++)
        {
            for (int x = left; x < right; x++)
            {
                // Get the color of both pixels at this point
                Color colorA = dataA[x - Rect.Left + (y - Rect.Top) * Rect.Width];
                Color colorB = dataB[x - obj.Rect.Left + (y - obj.Rect.Top) * obj.Rect.Width];

                // If both pixels are not completely transparent,
                if (colorA.A != 0 && colorB.A != 0)
                {
                    // then an intersection has been found
                    return true;
                }
            }
        }
        // No intersection found
        return false;
    }
    /// <summary>
    /// Return values: -1 : No collision, 0 = Hit ceiling, 1 = Hit ground
    /// </summary>
    /// <param name="tiles"></param>
    /// <returns></returns>
    public int UpdateFalling(List<Tile> tiles)
    {
        // NoClip: Keine Gravitation, bleibe auf gleicher Höhe
        if (NoClip.IsEnabled)
        {
            MoveSpeed = 0;
            return 1; // Als ob auf dem Boden stehend
        }

        MoveSpeed += Globals.Gravity;
        if (MoveSpeed > MaxFallSpeed)
            MoveSpeed = MaxFallSpeed;
        Rectangle nextPos = new((int)Pos.X, (int)(Pos.Y + Math.Ceiling(MoveSpeed) - 1), Width, Height + 1);
        foreach (var tile in tiles)
        {
            if (!nextPos.Intersects(tile.Rect)) continue;
            if (MoveSpeed < 0)
            {
                Pos = new Vector2(Pos.X, tile.Rect.Bottom + 1);
                MoveSpeed = 0;
                return 0;
            }
            else
            {
                Pos = new Vector2(Pos.X, tile.Rect.Top - Height);
                MoveSpeed = 0;
                return 1;
            }
        }
        Pos = new Vector2(Pos.X, Pos.Y + MoveSpeed);
        return -1;
    }
    public bool IsFalling(List<Tile> tiles)
    {
        if (UpdateFalling(tiles) == 1)
            return false;
        return true;
    }
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Pos, Color.White);
    }
}