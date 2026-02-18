
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Trap : Actor
{
    public override float MaxFallSpeed => 10f;
    public override int Height => Texture.Height;
    public override int Width => Texture.Width;
    public override int DamageOnOthers {get; init;} = 20;
    public override Texture2D Texture => TextureLoader.Texture[TextureID.Trap];
    public Trap(Vector2 pos)
    {
        Pos = new(pos.X, pos.Y + 60); // Adjust position to make trap align to ground
    }
    public override void Update(GameTime gameTime, Tile[,] map, double levelWidth)
    {
        UpdateFalling(GetNearbySolidTiles(map, 1));
    }
}