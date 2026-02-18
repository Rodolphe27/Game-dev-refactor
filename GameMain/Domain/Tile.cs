using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

public class Tile : GameObject
{
    public char Type;
    public bool IsSolid = false;
    public int Xindex => (int)(Pos.X / Globals.TileSize);
    public int Yindex => (int)(Pos.Y / Globals.TileSize);    public override int Width { get; set; } = Globals.TileSize;
    public override int Height { get; set; } = Globals.TileSize;
    private readonly static List<char> _SolidTypes = ['#'];
    public Tile(char tileType = ' ')
    {
        Type = tileType;
        Random random = new();
        if (Type == '#')
        {
            Texture = random.Next(0, 5) switch
            {
                0 => TextureLoader.Texture[TextureID.BrickTileMoss1],
                _ => TextureLoader.Texture[TextureID.BrickTile1],
            };
        }
        else if(Type == '_')
        {
            Texture = random.Next(0, 14) switch
            {
                0      => TextureLoader.Texture[TextureID.BrickTileBG1], // TEMP
                < 3 => TextureLoader.Texture[TextureID.BrickTileMossMG1],
                _      => TextureLoader.Texture[TextureID.BrickTileMG1],
            };
        }
        else if (Type == ' ' || Type == 'F')
        {
            Texture = random.Next(0, 60) switch
            {
                0 => TextureLoader.Texture[TextureID.BrickTileBG2],
                1 => TextureLoader.Texture[TextureID.BrickTileBG3],
                2 => TextureLoader.Texture[TextureID.BrickTileBG4],
                3 or 4 => TextureLoader.Texture[TextureID.BrickTileBG5],
                5 or 6 => TextureLoader.Texture[TextureID.BrickTileBG6],
                7 or 8 => TextureLoader.Texture[TextureID.BrickTileBG7],
                _ => TextureLoader.Texture[TextureID.BrickTileBG1],
            };
        }
        else if (Type == '+')
        {
            Texture = TextureLoader.Texture[TextureID.IronBarsTile1];
        }
        else if (Type == 'P')
        {
            Texture = TextureLoader.Texture[TextureID.Portal1];
            Width = Globals.TileSize * 2;
            Height = Globals.TileSize * 2;
        }
        else if (Type == 'Q')
        {
            Texture = TextureLoader.Texture[TextureID.Portal2];
            Width = Globals.TileSize * 2;
            Height = Globals.TileSize * 2;
        }
        else if (Type == 'B')
        {
            Texture = TextureLoader.Texture[TextureID.belohnungsPortal];
            Width = Globals.TileSize * 2;
            Height = Globals.TileSize * 2;
        }
        else if (Type == 'X')
        {
            Texture = TextureLoader.Texture[TextureID.SpiderWeb];
        }
        else
        {
            Texture = TextureLoader.Texture[TextureID.EmptyTile];
        }
        if(_SolidTypes.Contains(Type))
        {
            IsSolid = true;
        }
    }
    public static Tile[,] GenerateMap(char[,] charMap)
    {
        Tile[,] tileArray = new Tile[charMap.GetLength(0), charMap.GetLength(1)];
        for (int i = 0; i < charMap.GetLength(0); i++)
        {
            for (int j = 0; j < charMap.GetLength(1); j++)
            {
                Tile tile = new(charMap[i, j])
                {
                    Pos = new Vector2(j * Globals.TileSize, i * Globals.TileSize)
                };
                tileArray[i, j] = tile;
            }
        }
        return tileArray;
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Type == 'P' || Type == 'Q' || Type == 'B')
        {
            // Portal mit 2x Scale zeichnen
            spriteBatch.Draw(Texture, Pos, null, Color.White, 0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0f);
        }
        else
        {
            base.Draw(spriteBatch);
        }
    }
}