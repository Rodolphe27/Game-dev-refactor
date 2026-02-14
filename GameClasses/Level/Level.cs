using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class LevelClass
{
    public Tile[,] Map { get; private set; }
    public List<Actor> Actors = [];
    public double LevelHeight => Map.GetLength(0) * Globals.TileSize;
    public double LevelWidth => Map.GetLength(1) * Globals.TileSize;
    private char[,] TypeMap;
    public LevelClass()
    {
        TypeMap = WorldGenerator.GenerateWorld();
        Map = Tile.GenerateMap(TypeMap);
    }
    public LevelClass(char[,] typeMap)
    {
        TypeMap = typeMap;
        Map = Tile.GenerateMap(TypeMap);
    }
    
    public LevelClass(string levelPieceFile)
    {
        // Lade ein bestimmtes Level-Stück für Teleport-Ziel
        TypeMap = WorldGenerator.LoadLevelFromFile(levelPieceFile);
        Map = Tile.GenerateMap(TypeMap);
    }
    public void Initialise(Players player, bool spawnEnemies , int i)
    {
        List<Enemy> enemies = [];
        if (spawnEnemies)
        {
            switch(i){
                case 0:
                    // Create Spiders
                    Enumerable.Range(0, 20).ToList().ForEach(_ => enemies.Add(EnemyFactory.CreateEnemy(EnemyFactory.EnemyType.Spider, Map)));     
                    
                    // Create Ghosts
                    Enumerable.Range(0, 20).ToList().ForEach(_ => enemies.Add(EnemyFactory.CreateEnemy(EnemyFactory.EnemyType.Ghost, Map)));     

                    // Create Witches
                     enemies.Add(EnemyFactory.CreateEnemy(EnemyFactory.EnemyType.Witch, Map, EnemyFactory.CalculateWitchPositionNearPortal(TypeMap),1));
                    break;
                case 1:
                    // Create Spiders
                    Enumerable.Range(0, 20).ToList().ForEach(_ => enemies.Add(EnemyFactory.CreateEnemy(EnemyFactory.EnemyType.Spider, Map)));     
                    
                    // Create Ghosts
                    Enumerable.Range(0, 20).ToList().ForEach(_ => enemies.Add(EnemyFactory.CreateEnemy(EnemyFactory.EnemyType.Ghost, Map)));     

                    // Create Witches
                    enemies.Add(EnemyFactory.CreateEnemy(EnemyFactory.EnemyType.Witch, Map, EnemyFactory.CalculateWitchPositionNearPortal(TypeMap),2));
                    break;
                case 2:
                    // Create Spiders
                    Enumerable.Range(0, 20).ToList().ForEach(_ => enemies.Add(EnemyFactory.CreateEnemy(EnemyFactory.EnemyType.Spider, Map)));     
                    
                    // Create Ghosts
                    Enumerable.Range(0, 20).ToList().ForEach(_ => enemies.Add(EnemyFactory.CreateEnemy(EnemyFactory.EnemyType.Ghost, Map)));     

                    // Create Witches
                   enemies.Add(EnemyFactory.CreateEnemy(EnemyFactory.EnemyType.Witch, Map, EnemyFactory.CalculateWitchPositionNearPortal(TypeMap),3));
                    break;
                default:
                    Enumerable.Range(0, 20).ToList().ForEach(_ => enemies.Add(EnemyFactory.CreateEnemy(EnemyFactory.EnemyType.Spider, Map)));     
                    
                    // Create Ghosts
                    Enumerable.Range(0, 20).ToList().ForEach(_ => enemies.Add(EnemyFactory.CreateEnemy(EnemyFactory.EnemyType.Ghost, Map)));     

                    // Create Witches
                  //  enemies.Add(EnemyFa
                  // ctory.CreateEnemy(EnemyFactory.EnemyType.Witch, Map, EnemyFactory.CalculateWitchPositionNearPortal(TypeMap),1));
                       
                    break;      


            }
            // OnDestroyed handlers
            foreach(Enemy e in enemies)
            {

                e.OnAttack += Actors.AddRange;
                e.OnDestroyed += () => {
                    Globals.EnemyDiedCounter++;
                    Items item = e.OnDeath(player);
                    if (item is not null) Actors.Add(item);
                };
            }
            // Add all enemies to list
            Actors.AddRange(enemies);   

            // Spawn traps randomly across the world
            SpawnTraps();
        }

        // Spawn any chests that are authored directly into the tile map
        SpawnChestsFromMap();
        //SpawnBossWitch(player);
    }

    

    private void SpawnBossWitch(Players player)
    {
        Vector2? witchPosition = EnemyFactory.CalculateWitchPositionNearPortal(TypeMap);
        
        if (witchPosition.HasValue)
        {
            Enemy witchBoss = EnemyFactory.CreateEnemy(EnemyFactory.EnemyType.Witch, Map, witchPosition);
            Actors.Add(witchBoss);
        }
    }

    /// <summary>
    /// Spawnt Traps an zufälligen freien Positionen in der Welt.
    /// </summary>
    private void SpawnTraps()
    {
        List<Vector2> trapPositions = WorldGenerator.GetRandomTrapSpawnPositions(TypeMap);
        
        foreach (Vector2 pos in trapPositions)
        {
            Actors.Add(new Trap(pos));
        }
    }

    private void SpawnChestsFromMap()
    {
        for (int y = 0; y < TypeMap.GetLength(0); y++)
        {
            for (int x = 0; x < TypeMap.GetLength(1); x++)
            {
                if (TypeMap[y, x] == 'C')
                {
                    Vector2 pos = new Vector2(x * Globals.TileSize, y * Globals.TileSize);
                    Actors.Add(new Chest(pos));
                }
            }
        }
    }
    public void Update(GameTime gameTime, Players player)
    {
        for (int i = Actors.Count - 1; i >= 0; i--)
        {
            if (Actors[i].HealthPoints <= 0)
            {
                if(Actors[i] is Witch) player.KilledWitches++;
                Actors[i].OnDestroy();
                Actors.RemoveAt(i);
            }
        }
        for(int i = 0; i < Actors.Count; i++) Actors[i].Update(gameTime, Map, LevelWidth);

        Actors.Where(actor => actor is Enemy || actor is Attack { PlayerAttack: false }).ToList().ForEach(actor =>
        {
            if(actor is Attack attack && attack.IsProjectile && attack.GetHitShape().Intersects(player.Rect))
            {
                player.ReceiveDamage(attack.DamageOnOthers);
                attack.HealthPoints = -1;
            }
            else if(actor is Enemy enemy && enemy.IntersectPixels(player))
            {
                player.ReceiveDamage(enemy.DamageOnOthers);
            }
        });
        foreach (var actor in Actors)
        {
            if (actor is Attack { PlayerAttack: true } attack)
            {
                foreach (var enemy in Actors.OfType<Enemy>())
                {
                    if (attack.IsProjectile && attack.GetHitShape().Intersects(enemy.Rect))
                    {
                        enemy.ReceiveDamage(attack.DamageOnOthers);
                        attack.HealthPoints = -1;
                    }
                    if(!attack.IsProjectile && attack.IntersectPixels(enemy))
                    {
                        enemy.ReceiveDamage(attack.DamageOnOthers);
                    }
                }
            }
            if(actor is Items item && player.Rect.Intersects(item.Hitbox))
            {
                item.CollectItem(player);
            }
            if(actor is Trap trap && player.Rect.Intersects(trap.Rect))
            {
                if(player.HasReceivedDamage) continue;
                player.ReceiveDamage(trap.DamageOnOthers);
                trap.HealthPoints = -1;
            }
        }
    }
    public void Draw(SpriteBatch spriteBatch, List<Tile> nearbyTiles)
    {
        nearbyTiles.Where(tile => !tile.IsSolid && tile.Type != 'P' && tile.Type != 'Q' && tile.Type != 'B').ToList().ForEach(tile => { tile.Draw(spriteBatch); });
        foreach (Actor actor in Actors) actor.Draw(spriteBatch);
        nearbyTiles.Where(tile => tile.IsSolid && tile.Type != 'P' && tile.Type != 'Q' && tile.Type != 'B').ToList().ForEach(tile => { tile.Draw(spriteBatch); });
        // Portale zuletzt zeichnen, damit sie über allem anderen liegen (P, Q und B)
        nearbyTiles.Where(tile => tile.Type == 'P' || tile.Type == 'Q' || tile.Type == 'B').ToList().ForEach(tile => { tile.Draw(spriteBatch); });
    }
}