
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class Background(ContentManager content, string fileName) {
    ContentManager Content = content;
    Texture2D Texture = content.Load<Texture2D>(fileName);
    Vector2 Position = new(0,0);

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }

    internal void Update(double levelHeight, double levelWidth, Vector2 playerPos)
    {
        Position = new Vector2(playerPos.X / 2, 0);
    }
}