using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class EnemyFactory
{

    public enum EnemyType{ Spider, Ghost, Witch}
    public static Enemy CreateEnemy(EnemyType type, Tile[,] map, Vector2? customPosition = null, int levelid = 0)
    {
        Enemy enemy = type switch
        {
            EnemyType.Spider => new Spider(),
            EnemyType.Ghost => new Ghost(),
            EnemyType.Witch => new Witch(levelid),

            // Add other enemy types here
            _ => throw new Exception("invalid enemy type")
        };
        
    
    if (customPosition.HasValue)
        {
            enemy.Pos = customPosition.Value;
        }
        else
     {
            enemy.Pos = GetRandomGroundPosition(map, enemy);
        }
        
        return enemy;
    }
    
    public static Vector2? CalculateWitchPositionNearPortal(char[,] typeMap)
    {
        List<Vector2> portalPositions = [];
        
        for (int y = 0; y < typeMap.GetLength(0); y++)
        {
            for (int x = 0; x < typeMap.GetLength(1); x++)
            {
                if (typeMap[y, x] == 'P')
                {
                    portalPositions.Add(new Vector2(x * Globals.TileSize, y * Globals.TileSize));
                }
            }
        }
        
        if (portalPositions.Count > 0)
        {
            Random rnd = new Random();
            Vector2 portalPos = portalPositions[rnd.Next(portalPositions.Count)];
            return new Vector2(portalPos.X + Globals.TileSize * 3, portalPos.Y + Globals.TileSize * 2);
        }
        
        return null;
    }
    private static Vector2 GetRandomGroundPosition(Tile[,] map, Enemy enemy)
    {
        Random rnd = new Random();

        int tileX, tileY;
        int width = map.GetLength(1);
        int height = map.GetLength(0);

        float maxDistance = 600f;
        int maxAttempts = 100;


        for ( int i = 0; i< maxAttempts; i++)
        {
            tileX = rnd.Next(0, width);

            for (tileY = 0; tileY < height; tileY++)
            {
                Tile tile = map[tileY, tileX];

                if (tile.IsSolid)
                {
                    Vector2 candidatePos =  new Vector2(
                        tileX * Globals.TileSize,
                        tile.Rect.Top - enemy.Height
                    );

                    if(Vector2.Distance(candidatePos, Globals.players.Pos)> maxDistance)
                    {
                        return candidatePos;
                    }
                    else
                    {
                        break;
                    }
                }
                    }
                }
                return new Vector2(100, 100);
            }
       
       
       
        }
    
