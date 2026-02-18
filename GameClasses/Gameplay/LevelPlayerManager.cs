using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class LevelPlayerManager
{
    private Players Player;
    public float PlayerX => Player.Pos.X;
    public float PlayerY => Player.Pos.Y;
    public float PlayerWidth => Player.Width;
    public float Playerheight => Player.Height;
    public double LevelWidth => CurrLevel().LevelWidth;
    public double LevelHeight => CurrLevel().LevelHeight;
    private readonly List<LevelClass> Levels = [];
    private int LevelIndex = 0;
    private double _portalCooldown = 0;
    private bool[] _rewardUsedPerLevel; // Belohnungsraum pro Map nur einmal
    
    // Vorheriger Level und Position für TinyTest Rückkehr
    private int _previousLevelIndex = 0;
    private Vector2 _previousPlayerPos = Vector2.Zero;
    
    // TinyTest Timer und einmaliger Zugang
    private double _tinyTestTimer = 0;
    private const double TINYTEST_MAX_TIME = 60000; // 60 Sekunden in Millisekunden
    private bool _tinyTestUsed = false; // Permanent gesperrt nach einmaligem Besuch

    // Variables to steadily strengthen enemies
    private float ElapsedGameTime = 0f;
    private readonly int StrengthenInterval = 15000;
    
    public LevelPlayerManager(Players player)
    {
        Player = player;
        Player.Initialise();
        Player.CreateAttack += Player_CreateAttack;
        Levels = [
            new LevelClass(),
            new LevelClass(WorldGenerator.MapLevel2()), // Portal-Ziellevel
            new LevelClass(WorldGenerator.MapLevel3()), // Jump-Challenge Level
            new LevelClass(WorldGenerator.LoadLevelFromFile("TinyTest.txt")), // Training Map
            new LevelClass(WorldGenerator.LoadLevelFromFile("BelohnungsRoom.txt")) // Belohnungsraum
        ];
        _rewardUsedPerLevel = new bool[Levels.Count];
        for (int i = 0; i < Levels.Count; i++)
        {
            bool spawnEnemies = i != 4; // Index 4 is BelohnungsRoom - spawn-free zone
            Levels[i].Initialise(player, spawnEnemies,i);
        }
    }
    private void StrengthenEnemies(GameTime gameTime)
    {
        ElapsedGameTime += gameTime.ElapsedGameTime.Milliseconds;
        if (ElapsedGameTime >= StrengthenInterval)
        {
            ElapsedGameTime = 0f;
            foreach (LevelClass level in Levels)
            {
                foreach (Enemy enemy in level.Actors.OfType<Enemy>())
                {
                    enemy.HealthPoints += 2;
                    if(enemy is Witch) enemy.HealthPoints += 3;
                }
            }
        }
    }
    private void Player_CreateAttack(Attack[] attacks)
    {
        foreach(Attack attack in attacks)
        {
            CurrLevel().Actors.Add(attack);   
        }
    }
    private LevelClass CurrLevel()
    {
        return Levels[LevelIndex];
    }
    public Players CurrPlayer()
    {
        return Player;
    }
    public void Update(GameTime gameTime)
    {
        Player.Update(gameTime, CurrLevel().Map, LevelWidth);
        CurrLevel().Update(gameTime, Player);
        // Gameplay-Zeit pausieren, wenn TinyTest aktiv ist
        bool inTinyTest = LevelIndex == 3;

        // Gegner-Stärkung nur laufen lassen, wenn nicht im TinyTest (Gameplay-Zeit pausiert)
        if (!inTinyTest)
        {
            StrengthenEnemies(gameTime);
        }
        
        bool inRewardRoom = LevelIndex == 4;

        // TinyTest Timer verwalten
        if (inTinyTest) // Wenn in TinyTest
        {
            _tinyTestTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            Globals.PauseTime = true; // Gameplay-Zeit anhalten
            Globals.TinyTestActive = true;
            Globals.TinyTestRemainingMs = Math.Max(0, TINYTEST_MAX_TIME - _tinyTestTimer);
            
            // Nach 100 Sekunden automatisch zurückteleportieren
            if (_tinyTestTimer >= TINYTEST_MAX_TIME && _portalCooldown <= 0)
            {
                LevelIndex = _previousLevelIndex;
                Player.Pos = _previousPlayerPos;
                _tinyTestUsed = true; // Permanent sperren
                _tinyTestTimer = 0;
                _portalCooldown = 250; // 0.5s Cooldown
                Globals.PauseTime = false;
                Globals.TinyTestActive = false;
                Globals.TinyTestRemainingMs = 0;
            }
        }
        else
        {
            // Timer nur im TinyTest aktiv, sonst zurücksetzen
            _tinyTestTimer = 0;
            Globals.TinyTestActive = false;
            Globals.TinyTestRemainingMs = 0;
        }

        // Gameplay-Zeit auch im Belohnungsraum anhalten
        if (inRewardRoom)
        {
            Globals.PauseTime = true;
        }
        else if (!inTinyTest)
        {
            Globals.PauseTime = false;
        }
        
        // Portal-Cooldown reduzieren
        if (_portalCooldown > 0)
            _portalCooldown -= gameTime.ElapsedGameTime.TotalMilliseconds;
        
        // Portal-Erkennung: Prüfe ob Spieler nahe am Portal 'P', 'Q' oder 'B' steht (1 Tile Radius)
        int tileX = (int)(Player.Pos.X / Globals.TileSize);
        int tileY = (int)(Player.Pos.Y / Globals.TileSize);
        
        bool onPortalP = false;
        bool onPortalQ = false;
        bool onPortalB = false;
        
        // Prüfe aktuelle Tile und benachbarte Tiles (Radius 1)
        for (int y = tileY - 1; y <= tileY + 1; y++)
        {
            for (int x = tileX - 1; x <= tileX + 1; x++)
            {
                if (y >= 0 && y < CurrLevel().Map.GetLength(0) && 
                    x >= 0 && x < CurrLevel().Map.GetLength(1))
                {
                    char tileType = CurrLevel().Map[y, x].Type;
                    if (tileType == 'P')
                        onPortalP = true;
                    else if (tileType == 'Q')
                        onPortalQ = true;
                    else if (tileType == 'B')
                        onPortalB = true;
                }
            }
        }
        
        if (_portalCooldown <= 0)
        {
            // Portal 'P' - Level-Übergänge
            if (onPortalP)
            {
                // Von Level 0 zu Level 1 (wenn Spieler 1 Schlüssel hat)
                if (LevelIndex == 0 && Player.CanUsePortal())
                {
                    Player.NumberKeyGold--; // Verliere einen goldenen Key
                    Globals.PortalsUsed++;
                    LevelIndex = 1;
                    // Level 2: Spieler startet in TinyTest (oben links)
                    // TinyTest ist auf der linken Seite oben
                    // NoiseMap (80 breit) + TinyTest/Cave darüber
                    double startX = 80 * Globals.TileSize + 10 * Globals.TileSize;
                    double startY = 5 * Globals.TileSize;
                    Player.Pos = new Vector2((float)startX, (float)startY);
                    _portalCooldown = 250; // 0.5s Cooldown
                }
                // Von Level 1 zu Level 2 (Jump Challenge)
                else if (LevelIndex == 1 && Player.CanUsePortal())
                {
                    Player.NumberKeyGold--; // Verliere einen goldenen Key
                    Globals.PortalsUsed++;
                    LevelIndex = 2;
                    // Level 3: Spieler startet bei MapEntry (50 Kacheln links von der Mitte)
                    double midX = CurrLevel().LevelWidth / 2 - 50 * Globals.TileSize;
                    Player.Pos = new Vector2((float)midX, 6 * Globals.TileSize);
                    _portalCooldown = 250; // 0.5s Cooldown
                }
            }
            
            // Portal 'Q' - TinyTest Training (nur einmaliger Zugang)
            if (onPortalQ && Player.CanUsePortal() && !_tinyTestUsed)
            {
                Player.NumberKeyGold--; // Verliere einen goldenen Key
                // Speichere vorherigen Level und Position
                _previousLevelIndex = LevelIndex;
                _previousPlayerPos = Player.Pos;
                
                LevelIndex = 3; // TinyTest Level
                // Spieler spawnt in der Mitte von TinyTest
                double midX = CurrLevel().LevelWidth / 2;
                Player.Pos = new Vector2((float)midX, 6 * Globals.TileSize);
                _tinyTestTimer = 0; // Timer starten
                _portalCooldown = 250; // 0.5s Cooldown
                Globals.PauseTime = true;
                Globals.TinyTestActive = true;
                Globals.TinyTestRemainingMs = TINYTEST_MAX_TIME;
            }

            // Portal 'B' - Belohnungsraum (Hin- und Rückweg)
            if (onPortalB)
            {
                const int rewardLevelIndex = 4; // Index des Belohnungsraums in Levels
                if (LevelIndex != rewardLevelIndex)
                {
                    if (_rewardUsedPerLevel[LevelIndex])
                        return; // Pro Map nur einmal

                    if (Player.NumberKeySilver <= 0)
                        return; // Silberner Schlüssel erforderlich

                    Player.NumberKeySilver--; // Verliere einen silbernen Key beim Eintreten
                    // Eintritt in den Belohnungsraum: vorherigen Zustand merken
                    _previousLevelIndex = LevelIndex;
                    _previousPlayerPos = Player.Pos;
                    _rewardUsedPerLevel[LevelIndex] = true; // pro Map nur einmal

                    // Belohnungsraum frisch instanziieren, damit Kisten resetten
                    Levels[rewardLevelIndex] = new LevelClass(WorldGenerator.LoadLevelFromFile("BelohnungsRoom.txt"));
                    Levels[rewardLevelIndex].Initialise(Player, spawnEnemies: false, rewardLevelIndex);

                    LevelIndex = rewardLevelIndex;
                    // Spieler spawnt unten links im Belohnungsraum
                    double startX = 5 * Globals.TileSize;
                    double startY = CurrLevel().LevelHeight - 5 * Globals.TileSize;
                    Player.Pos = new Vector2((float)startX, (float)startY);
                    _portalCooldown = 250; // 0.5s Cooldown
                }
                else
                {
                    // Rückweg: zurück in vorherigen Level und Position (kostenlos, ohne Key-Anforderung)
                    LevelIndex = _previousLevelIndex;
                    Player.Pos = _previousPlayerPos;
                    _portalCooldown = 250; // 0.5s Cooldown
                }
            }
        }
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        CurrLevel().Draw(spriteBatch, Player.GetNearbyTiles(CurrLevel().Map, 50));
        Player.Draw(spriteBatch);
    }
}