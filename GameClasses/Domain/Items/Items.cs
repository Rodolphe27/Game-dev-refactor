
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class Items : Actor
{
    public override float MaxFallSpeed => 10f;
    public override int Height => Texture.Height;
    public override int Width => Texture.Width;
    public Rectangle Hitbox => CalculateItemHitbox();
    public Rectangle CalculateItemHitbox()
    {
        float DeltaHeight = Globals.TileSize - Height;
        float BoxHeight = Height + DeltaHeight + 1;
        return new Rectangle((int)Pos.X, (int)(Pos.Y - DeltaHeight - 1), Width, (int)BoxHeight);
    }
    public abstract void CollectItem(Players player);
    public Items(Vector2 pos)
    {
        float DeltaHeight = Globals.TileSize - Height;
        Pos = new Vector2(pos.X, pos.Y + DeltaHeight);
    }
    public override void Update(GameTime gameTime, Tile[,] map, double levelWidth)
    {
        UpdateFalling(GetNearbySolidTiles(map, 1));
    }
}