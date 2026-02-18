using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;

/// <summary>
/// Generates the game world layout as a 2D array of tile types.
/// TODO: Implement procedural generation later.
/// </summary>
public class WorldGenerator() 
{
    /// <summary>
    /// Returns a 2D int array of tile types. TODO: Implement procedural generation later.
    /// </summary>
    /// <returns></returns>
    static string pathOfStaticMap = Path.Combine(AppContext.BaseDirectory, "Content/LevelPieces", "Map1Start.txt");
    static string[] linesOfStaticMap = File.ReadAllLines(pathOfStaticMap);
    static int heightStaticofMap = linesOfStaticMap.Length;
    public static char[,] GenerateWorld()
    {
    
            // Top row: [staticMap][caveMap][NoiseMap]
            char[,] staticMap = LoadLevelFromFile("Map1Start.txt");
            char[,] caveStaticMap = LoadLevelFromFile("Map1Cave.txt");
            int caveWidth = caveStaticMap.GetLength(1);

            // 1. Procedural Cave erzeugen
            char[,] caveProcedural = GenerateCaveMap(caveWidth, heightStaticofMap);

            // 2. Cave.txt darunter setzen
            char[,] caveCombinedVertical = CombineVertical(caveProcedural, caveStaticMap);
            
            // 3. NoiseMap generieren
            char[,] dynamicArea = GenerateNoiseMap(80, 30);
            PlaceRewardPortalInNoise(dynamicArea, 12); // Portal B zwei Tiles über dem Boden platzieren

            // 4. Top row zusammenführen
            char[,] topRow = CombineHorizontalWithStairs(staticMap, caveCombinedVertical);
            topRow = CombineHorizontal(topRow, dynamicArea);

            // Bottom row: [BossRoomLeft][StaticCaveMap][BossRoomRight]
            char[,] bossLeft = LoadLevelFromFile("BossRoomLeft.txt");
            char[,] bossRight = LoadLevelFromFile("BossRoomRight.txt");
            char[,] staticCave = LoadLevelFromFile("Map1Cave1.txt");

            char[,] bottomRow = CombineHorizontal(bossLeft, staticCave);
            bottomRow = CombineHorizontal(bottomRow, bossRight);

            // Final: zwei Reihen vertikal stapeln
            char[,] world = CombineVertical(topRow, bottomRow);

            // Abschließend: Karte mit einem Wandrahmen umhüllen und Löcher füllen
            // char[,] wrapped = WrapWithBorder(world);

            // Spinnenweben in Hohlräumen platzieren
            PlaceSpiderWebs(world);

        return world;
    }
    /// <summary>
    /// Zweite Welt: anderes Layout mit selben Bausteinen (PortalDest/Cave/Noise/BossRooms), Spalten-basiert.
    /// </summary>
    /// <summary>
    /// Level 2: Spieler startet in Map2Start, kann nach links (Cave + Noise) und rechts (Portal + Boss)
    /// </summary>
    public static char[,] MapLevel2()
    {
        int standardHeight = 30;
        
        // Linke Seite: NoiseMap + (TinyTest über Cave.txt)
        char[,] noiseLeft = GenerateNoiseMap(80, standardHeight);
        PlaceRewardPortalInNoise(noiseLeft, 12);
        char[,] tinyTest = LoadLevelFromFile("Map2Start.txt");
        char[,] caveLeft = LoadLevelFromFile("Map2Cave2.txt");
        char[,] caveWithTinyTest = CombineVertical(tinyTest, caveLeft);
        char[,] leftSide = CombineHorizontal(noiseLeft, caveWithTinyTest);
        
        // Mitte: Map2Start auf gleicher Höhe wie Cave (unten) - TinyTest über Map2Start
        char[,] map2StartBase = LoadLevelFromFile("Map2Middle.txt");
        int tinyTestHeight = tinyTest.GetLength(0);
        int map2StartWidth = map2StartBase.GetLength(1);
        // Erstelle leeren Raum über Map2Start mit gleicher Höhe wie TinyTest
        char[,] emptyAbove = new char[tinyTestHeight, map2StartWidth];
        for (int y = 0; y < tinyTestHeight; y++)
            for (int x = 0; x < map2StartWidth; x++)
                emptyAbove[y, x] = ' ';
        char[,] map2Start = CombineVertical(emptyAbove, map2StartBase);
        
        // Rechte Seite: MapEntry (Portal) + Bossraum - auch mit leerem Raum oben
        char[,] mapEntryBase = LoadLevelFromFile("MapEntry.txt");
        char[,] bossRoom = LoadLevelFromFile("BossRoomRight.txt");
        char[,] rightSideBase = CombineHorizontal(mapEntryBase, bossRoom);
        int rightWidth = rightSideBase.GetLength(1);
        char[,] emptyAboveRight = new char[tinyTestHeight, rightWidth];
        for (int y = 0; y < tinyTestHeight; y++)
            for (int x = 0; x < rightWidth; x++)
                emptyAboveRight[y, x] = ' ';
        char[,] rightSide = CombineVertical(emptyAboveRight, rightSideBase);
        
        // Kombiniere: Links (Noise+Cave) + Mitte (Map2Start) + Rechts (Portal+Boss)
        char[,] world = CombineHorizontalWithStairs(leftSide, map2Start);
        world = CombineHorizontalWithStairs(world, rightSide);

        // Spinnenweben in Hohlräumen platzieren
        PlaceSpiderWebs(world);

        return world;
    }

    /// <summary>
    /// Level 3: Jump-Challenge mit vertikalen Plattformen
    /// Spieler startet rechts und muss nach LINKS laufen
    /// Nutzt dynamische NoiseMap und CaveMap mit statischem MapEntry Portal
    /// </summary>
    public static char[,] MapLevel3()
    {
        int standardHeight = 30;
        
        // Rechte Seite: Dynamisches Startgebiet (wo der Spieler spawnt)
        char[,] noiseStart = GenerateNoiseMap(100, standardHeight);
        char[,] caveStart = GenerateCaveMap(100, standardHeight);
        char[,] rightStart = CombineVertical(noiseStart, caveStart);
        
        // Mittlerer Bereich: Jump-Challenge Plattformen + MapEntry mit Portal
        char[,] map3Middle = LoadLevelFromFile("Map3Middle.txt");
        char[,] mapEntry = LoadLevelFromFile("MapEntry.txt");
        char[,] middleWithEntry = CombineHorizontal(map3Middle, mapEntry);
        
        // Linker Bereich: Finale Challenge (Ziel) + BossRoomRight
        char[,] map3End = LoadLevelFromFile("Map3End.txt");
        char[,] bossRoom = LoadLevelFromFile("BossRoomRight.txt");
        char[,] leftEnd = CombineHorizontal(map3End, bossRoom);
        
        // Kombiniere: Rechts (Start) + Mitte+Entry (Challenge) + Links (Ziel+Boss)
        char[,] world = CombineHorizontalWithStairs(rightStart, middleWithEntry);
        world = CombineHorizontalWithStairs(world, leftEnd);

        // Spinnenweben in Hohlräumen platzieren
        PlaceSpiderWebs(world);

        return world;
    }

    public static char[,] LoadLevelFromFile(string fileName)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Content/LevelPieces", fileName);
        string[] lines = File.ReadAllLines(path);

        int height = lines.Length;
        int width = 0;

        // Maximale Breite finden
        foreach (var line in lines)
            if (line.Length > width)
                width = line.Length;

        char[,] level = new char[height, width];

        for (int y = 0; y < height; y++)
        {
            string line = lines[y].PadRight(width, ' '); // fehlende Zeichen auffüllen
            for (int x = 0; x < width; x++)
                level[y, x] = line[x];
        }

        return level;
    }

    private static void PlaceRewardPortalInNoise(char[,] noiseMap, int portalX)
    {
        int height = noiseMap.GetLength(0);
        int width = noiseMap.GetLength(1);
        if (portalX < 0 || portalX >= width)
            return;

        // Finde die Oberfläche ('_') an der X-Position und platziere 2 Tiles darüber
        for (int y = 0; y < height; y++)
        {
            if (noiseMap[y, portalX] == '_')
            {
                int targetY = y - 2; // zwei Kacheln über der Oberfläche
                if (targetY >= 0)
                {
                    // Falls dort kein Platz ist, nimm einen Tile höher
                    if (noiseMap[targetY, portalX] != ' ' && targetY - 1 >= 0)
                        targetY -= 1;

                    if (noiseMap[targetY, portalX] == ' ')
                    {
                        noiseMap[targetY, portalX] = 'B';
                    }
                }
                break;
            }
        }
    }
public static char[,] GenerateCaveMap(int width, int height)
{
    char[,] map = new char[height, width];
    Random rnd = new Random();

    // Schritt 1: Alles mit Wänden füllen
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            map[y, x] = '#';
        }
    }

    // Schritt 2: Hauptweg von links nach rechts carven (garantierter Durchgang)
    int currentX = 0;  // Start am linken Rand
    int currentY = height / 2;
    
    while (currentX < width)  // Bis zum rechten Rand
    {
        // Öffne einen Bereich um die aktuelle Position
        for (int dy = -3; dy <= 3; dy++)  // Größerer Durchgang
        {
            int y = currentY + dy;
            if (y >= 0 && y < height)
            {
                for (int dx = 0; dx < 5; dx++)
                {
                    int x = currentX + dx;
                    if (x >= 0 && x < width)
                    {
                        map[y, x] = ' ';
                    }
                }
            }
        }

        // Bewegung nach rechts mit zufälligen vertikalen Bewegungen
        currentX += rnd.Next(3, 6);
        
        // Zufällige vertikale Bewegung
        int verticalChange = rnd.Next(-2, 3);
        currentY = Math.Max(3, Math.Min(height - 4, currentY + verticalChange));
    }
    
    // Öffne explizit den linken und rechten Rand für den Durchgang
    for (int y = height / 2 - 4; y <= height / 2 + 4; y++)
    {
        if (y >= 0 && y < height)
        {
            // Linker Rand offen
            for (int x = 0; x < 5; x++)
            {
                map[y, x] = ' ';
            }
            // Rechter Rand offen
            for (int x = width - 5; x < width; x++)
            {
                map[y, x] = ' ';
            }
        }
    }

    // Schritt 3: Sackgassen hinzufügen
    for (int i = 0; i < 5; i++)
    {
        int startX = rnd.Next(10, width - 20);
        int startY = rnd.Next(2, height - 2);
        
        // Suche einen Punkt nahe dem Hauptweg
        bool foundNearMain = false;
        for (int checkX = startX - 5; checkX <= startX + 5; checkX++)
        {
            for (int checkY = startY - 3; checkY <= startY + 3; checkY++)
            {
                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                {
                    if (map[checkY, checkX] == ' ')
                    {
                        foundNearMain = true;
                        startX = checkX;
                        startY = checkY;
                        break;
                    }
                }
            }
            if (foundNearMain) break;
        }

        if (!foundNearMain) continue;

        // Carve Sackgasse
        int sackX = startX;
        int sackY = startY;
        int sackLength = rnd.Next(5, 15);

        for (int step = 0; step < sackLength; step++)
        {
            // Öffne Bereich
            for (int dy = -1; dy <= 1; dy++)
            {
                int y = sackY + dy;
                if (y >= 0 && y < height)
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        int x = sackX + dx;
                        if (x >= 0 && x < width)
                        {
                            map[y, x] = ' ';
                        }
                    }
                }
            }

            // Bewegung in zufällige Richtung
            int direction = rnd.Next(4);
            if (direction == 0) sackX += 2;      // Rechts
            else if (direction == 1) sackX -= 2; // Links
            else if (direction == 2) sackY -= 1; // Oben
            else sackY += 1;                      // Unten

            // In Grenzen halten
            sackX = Math.Max(2, Math.Min(width - 2, sackX));
            sackY = Math.Max(2, Math.Min(height - 2, sackY));
        }
    }

    // Schritt 4: Zusätzliche organische Räume
    for (int i = 0; i < 3; i++)
    {
        int roomX = rnd.Next(10, width - 20);
        int roomY = rnd.Next(2, height - 5);
        int roomWidth = rnd.Next(5, 10);
        int roomHeight = rnd.Next(3, 6);

        for (int y = roomY; y < roomY + roomHeight && y < height; y++)
        {
            for (int x = roomX; x < roomX + roomWidth && x < width; x++)
            {
                map[y, x] = ' ';
            }
        }
    }

    // Schritt 5: Cellular Automata für organische Struktur
    for (int i = 0; i < 2; i++)
    {
        map = SmoothCave(map);
    }

    // Schritt 6: Boden '_' setzen
    for (int y = 1; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (map[y, x] == ' ' && map[y - 1, x] == '#')
                map[y, x] = '_';
        }
    }

    return map;
}

/// <summary>
/// Sanftere Glättung für Höhlen - behält den Hauptweg
/// </summary>
private static char[,] SmoothCave(char[,] oldMap)
{
    int width = oldMap.GetLength(1);
    int height = oldMap.GetLength(0);
    char[,] newMap = new char[height, width];

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            int walls = CountWalls(oldMap, x, y);

            // Sanftere Schwelle - behält mehr Raum offen
            if (walls > 5)
                newMap[y, x] = '#';
            else
                newMap[y, x] = ' ';
        }
    }

    return newMap;
}   
public static char[,] GenerateNoiseMap(int width, int height)
{
    char[,] map = new char[height, width];
    Random rnd = new Random();

    float smoothness = 0.1f;

    for (int x = 0; x < width; x++)
    {
        // erzeugt Terrainhöhe
        int groundHeight = (int)(Math.Sin(x * smoothness) * 5 + height / 2);

        for (int y = 0; y < height; y++)
        {
            if (y < groundHeight)
                map[y, x] = ' '; // Luft
            else if (y == groundHeight)
                map[y, x] = '_'; // Oberfläche
            else
                map[y, x] = '#'; // Fels
        }
    }
    return map;
}
private static char[,] Smooth(char[,] oldMap)
{
    int width = oldMap.GetLength(1);
    int height = oldMap.GetLength(0);

    char[,] newMap = new char[height, width];

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            int walls = CountWalls(oldMap, x, y);

            if (walls > 4)
                newMap[y, x] = '#'; // Wand
            else
                newMap[y, x] = ' '; // Luft
        }
    }

    return newMap;
}

private static int CountWalls(char[,] map, int x, int y)
{
    int width = map.GetLength(1);
    int height = map.GetLength(0);
    int count = 0;

    for (int yy = y - 1; yy <= y + 1; yy++)
    {
        for (int xx = x - 1; xx <= x + 1; xx++)
        {
            if (xx < 0 || yy < 0 || xx >= width || yy >= height)
            {
                count++; // Rand → Wand
            }
            else if (map[yy, xx] == '#')
            {
                count++;
            }
        }
    }

    return count;
}
    /// <summary>
    /// Öffnet einen einfachen rechteckigen Durchgang an der Naht Portal->Cave.
    /// </summary>
    private static void CreatePortalToCavePassage(char[,] map, int seamX, int centerY)
    {
        int passageWidth = 42; // 40% breiter als zuvor
        int passageHeight = 6;
        int startX = Math.Max(0, seamX - passageWidth / 2);
        int startY = Math.Max(1, centerY - passageHeight / 2);

        for (int y = 0; y < passageHeight; y++)
        {
            for (int x = 0; x < passageWidth; x++)
            {
                int tx = startX + x;
                int ty = startY + y;
                if (ty >= 0 && ty < map.GetLength(0) && tx >= 0 && tx < map.GetLength(1))
                {
                    // Untere Reihe als Boden
                    if (y == passageHeight - 1)
                        map[ty, tx] = '_';
                    else
                        map[ty, tx] = ' ';
                }
            }
        }
    }
public static char[,] CombineVertical(char[,] top, char[,] bottom)
{
    int topH = top.GetLength(0);
    int topW = top.GetLength(1);
    int botH = bottom.GetLength(0);
    int botW = bottom.GetLength(1);

    int finalH = topH + botH;
    int finalW = Math.Max(topW, botW);

    char[,] result = new char[finalH, finalW];

    // Initial mit Wänden füllen, um Löcher (\0) zu vermeiden
    for (int y = 0; y < finalH; y++)
        for (int x = 0; x < finalW; x++)
            result[y, x] = '#';

    // obere Karte
    for (int y = 0; y < topH; y++)
        for (int x = 0; x < topW; x++)
            result[y, x] = top[y, x];

    // untere Karte
    for (int y = 0; y < botH; y++)
        for (int x = 0; x < botW; x++)
            result[y + topH, x] = bottom[y, x];

    return result;
}
public static char[,] CombineHorizontal(char[,] left, char[,] right)
{
    int leftH = left.GetLength(0);
    int leftW = left.GetLength(1);
    int rightH = right.GetLength(0);
    int rightW = right.GetLength(1);

    int finalH = Math.Max(leftH, rightH);
    int finalW = leftW + rightW;

    char[,] result = new char[finalH, finalW];

    // Initial mit Wänden füllen, um Löcher (\0) zu vermeiden
    for (int y = 0; y < finalH; y++)
        for (int x = 0; x < finalW; x++)
            result[y, x] = '#';

    // links kopieren
    for (int y = 0; y < leftH; y++)
        for (int x = 0; x < leftW; x++)
            result[y, x] = left[y, x];

    // rechts kopieren
    for (int y = 0; y < rightH; y++)
        for (int x = 0; x < rightW; x++)
            result[y, x + leftW] = right[y, x];

    // Durchgang zwischen den Bereichen erstellen
    CreatePassage(result, leftW, leftH, rightH, false);

    return result;
}

/// <summary>
/// Kombiniert zwei Karten horizontal mit Treppenerstellung (für staticMap + caveProcedural).
/// </summary>
public static char[,] CombineHorizontalWithStairs(char[,] left, char[,] right)
{
    int leftH = left.GetLength(0);
    int leftW = left.GetLength(1);
    int rightH = right.GetLength(0);
    int rightW = right.GetLength(1);

    int finalH = Math.Max(leftH, rightH);
    int finalW = leftW + rightW;

    char[,] result = new char[finalH, finalW];

    // Initial mit Wänden füllen, um Löcher (\0) zu vermeiden
    for (int y = 0; y < finalH; y++)
        for (int x = 0; x < finalW; x++)
            result[y, x] = '#';

    // links kopieren
    for (int y = 0; y < leftH; y++)
        for (int x = 0; x < leftW; x++)
            result[y, x] = left[y, x];

    // rechts kopieren
    for (int y = 0; y < rightH; y++)
        for (int x = 0; x < rightW; x++)
            result[y, x + leftW] = right[y, x];

    // Durchgang MIT Treppen erstellen (staticMap + caveProcedural)
    CreatePassage(result, leftW, leftH, rightH, true);

    return result;
}

/// <summary>
/// Erstellt einen Durchgang zwischen der Cave und der NoiseMap
/// </summary>
private static void CreatePassage(char[,] map, int passageX, int leftHeight, int rightHeight, bool createStairs = false)
{
    int mapHeight = map.GetLength(0);
    int mapWidth = map.GetLength(1);
    
    // Finde einen freien Bereich in der linken Seite (Cave) als Startpunkt
    int startY = -1;
    
    // Suche von unten nach oben nach einem freien Punkt - aber 5 Kacheln höher starten
    for (int y = leftHeight - 7; y >= 0; y--)
    {
        bool foundSpace = false;
        for (int checkX = passageX - 10; checkX < passageX; checkX++)
        {
            if (checkX >= 0 && checkX < mapWidth && map[y, checkX] == ' ')
            {
                foundSpace = true;
                break;
            }
        }
        if (foundSpace)
        {
            startY = y;
            break;
        }
    }

    // Fallback: Öffne einen Bereich 5 Kacheln höher in der Mitte
    if (startY == -1)
    {
        startY = Math.Max(5, leftHeight / 2 - 5);
    }

    // Breite und Höhe des Durchgangs
    int passageWidth = 6;
    int passageHeight = 5;
    int halfHeight = passageHeight / 2;

    // Durchgang öffnen - aber nur Wälle, kein Boden!
    for (int dy = -halfHeight; dy <= halfHeight; dy++)
    {
        int y = startY + dy;
        if (y >= 0 && y < mapHeight)
        {
            // Von rechts der linken Seite bis zur Mitte und darüber hinaus
            for (int dx = -6; dx < passageWidth + 2; dx++)
            {
                int x = passageX + dx;
                if (x >= 0 && x < mapWidth)
                {
                    // Öffne nur wenn es eine Wand ist, behalte Luft und Boden
                    if (map[y, x] == '#')
                    {
                        map[y, x] = ' '; // Wand zu Luft
                    }
                }
            }
        }
    }

    // Suche einen passenden Punkt in der NoiseMap auf der rechten Seite
    int noiseStartY = startY;
    
    // Versuche einen freien Punkt in der NoiseMap zu finden
    for (int y = startY - halfHeight; y <= startY + halfHeight; y++)
    {
        if (y >= 0 && y < mapHeight)
        {
            bool foundSpace = false;
            for (int checkX = passageX + 1; checkX < passageX + 8; checkX++)
            {
                if (checkX >= 0 && checkX < mapWidth && map[y, checkX] == ' ')
                {
                    foundSpace = true;
                    noiseStartY = y;
                    break;
                }
            }
            if (foundSpace) break;
        }
    }

    // Verbinde zur NoiseMap - auch hier nur Wälle öffnen
    for (int dy = -halfHeight; dy <= halfHeight; dy++)
    {
        int y = noiseStartY + dy;
        if (y >= 0 && y < mapHeight)
        {
            for (int dx = 0; dx < passageWidth + 2; dx++)
            {
                int x = passageX + dx;
                if (x >= 0 && x < mapWidth)
                {
                    // Nur Wälle öffnen - Boden und Luft bleiben
                    if (map[y, x] == '#')
                    {
                        map[y, x] = ' ';
                    }
                }
            }
        }
    }

    // Verbinde die Y-Positionen wenn sie unterschiedlich sind
    if (startY != noiseStartY)
    {
        int minY = Math.Min(startY, noiseStartY);
        int maxY = Math.Max(startY, noiseStartY);
        
        for (int y = minY; y <= maxY; y++)
        {
            for (int dx = -3; dx < passageWidth; dx++)
            {
                int x = passageX + dx;
                if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
                {
                    // Nur Wälle öffnen
                    if (map[y, x] == '#')
                    {
                        map[y, x] = ' ';
                    }
                }
            }
        }
    }

    // Erstelle eine Treppe am rechten Rand des Durchgangs nach unten (nur wenn gewünscht)
    if (createStairs)
    {
        // Finde die Position des Durchgangs auf der rechten Seite
        int stairX = passageX  + passageWidth - 1; // Rechter Rand des Durchgangs
        
        // Treppe beginnt oben vom Durchgang und führt nach unten
        int stairTopY = Math.Min(startY, noiseStartY) - 2; // Ein paar Blöcke über dem Durchgang
        int stairBottomY = mapHeight - 2;                  // Ganz nach unten zur ganzen Kartenhoehe
        
        int stairWidth = 4;  // Vergrößerte Breite
        int stepsCount = Math.Abs(stairBottomY - stairTopY);
        
        if (stepsCount > 0)
        {
            for (int step = 0; step <= stepsCount; step++)
            {
                // Interpoliere von oben nach unten
                int currentY = stairTopY + (step * (stairBottomY - stairTopY)) / stepsCount;
                int currentX = stairX + step; // Treppe geht nach rechts während sie heruntergeht
                
                // Öffne Bereich für die Treppenstufe - doppelt so groß
                for (int dy = -2; dy <= 2; dy++)
                {
                    for (int dx = 0; dx < stairWidth; dx++)
                    {
                        int x = currentX + dx;
                        int y = currentY + dy;
                        
                        if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
                        {
                            if (map[y, x] == '#')
                            {
                                map[y, x] = ' '; // Öffne Wand
                            }
                        }
                    }
                }
            }
        }
    }
}

/// <summary>
/// Umhüllt die gesamte Karte mit einer ein-Kachel-dicken Wand und füllt \0-Löcher.
/// </summary>
/// 
/// 
public static char[,] WrapWithBorder(char[,] map)
{
    int h = map.GetLength(0);
    int w = map.GetLength(1);

    // Neue Karte mit +2 in Höhe/Breite (Rahmen oben/unten/links/rechts)
    char[,] wrapped = new char[h + 2, w + 2];

    // Zuerst alles als Wand setzen
    for (int y = 0; y < h + 2; y++)
    {
        for (int x = 0; x < w + 2; x++)
        {
            wrapped[y, x] = '#';
        }
    }

    // Originalinhalt hinein kopieren, \0 als Wand behandeln
    for (int y = 0; y < h; y++)
    {
        for (int x = 0; x < w; x++)
        {
            char c = map[y, x];
            if (c == '\0') c = '#';
            wrapped[y + 1, x + 1] = c;
        }
    }

    return wrapped;
}

/// <summary>
/// Findet alle freien Plätze (Space/Luft) in der Karte und gibt deren Positionen zurück.
/// </summary>
/// <param name="map">Die zu durchsuchende Karte</param>
/// <returns>Liste mit Positionen (x, y) aller freien Plätze</returns>
public static List<(int x, int y)> FindFreeSpaces(char[,] map)
{
    List<(int x, int y)> freeSpaces = new();
    int height = map.GetLength(0);
    int width = map.GetLength(1);

    for (int y = 0; y < height - 1; y++) // -1 weil wir den Boden darunter prüfen
    {
        for (int x = 0; x < width; x++)
        {
            // Prüfe ob aktuelle Position Luft ist und darunter ein Boden
            if (map[y, x] == ' ' && (map[y + 1, x] == '_' || map[y + 1, x] == '#'))
            {
                freeSpaces.Add((x, y));
            }
        }
    }

    return freeSpaces;
}

/// <summary>
/// Gibt zufällige Positionen für Trap-Spawns zurück.
/// </summary>
/// <param name="map">Die Weltkarte</param>
/// <param name="trapCount">Anzahl der zu spawnenden Traps (Standard: 20% der freien Plätze)</param>
/// <param name="seed">Optional: Seed für den Zufallsgenerator</param>
/// <returns>Liste mit Vector2 Positionen für Traps</returns>
public static List<Vector2> GetRandomTrapSpawnPositions(char[,] map, int? trapCount = null, int? seed = null)
{
    List<(int x, int y)> freeSpaces = FindFreeSpaces(map);
    List<Vector2> trapPositions = new();
    
    if (freeSpaces.Count == 0)
        return trapPositions;

    Random rng = seed.HasValue ? new Random(seed.Value) : new Random();
    
    // Wenn keine Anzahl angegeben, spawne Traps auf ca. 2% der freien Plätze
    int actualTrapCount = trapCount ?? Math.Max(10, freeSpaces.Count / 50);
    actualTrapCount = Math.Min(actualTrapCount, freeSpaces.Count); // Nicht mehr als freie Plätze

    // Mische die freien Plätze und nimm die ersten N
    List<(int x, int y)> shuffledSpaces = freeSpaces.OrderBy(_ => rng.Next()).ToList();
    
    for (int i = 0; i < actualTrapCount; i++)
    {
        var (x, y) = shuffledSpaces[i];
        // Konvertiere Kachel-Position zu Pixel-Position (72 ist TileSize)
        trapPositions.Add(new Vector2(x * 72, y * 72));
    }

    return trapPositions;
}

/// <summary>
/// Erkennt Hohlräume (3+ freie Kacheln zwischen Wänden) und platziert Spinnenweben zufällig.
/// Ein Hohlraum ist definiert als: '#' gefolgt von 3+ freien Kacheln ' ' und dann wieder '#'.
/// </summary>
/// <param name="map">Die Weltkarte, die modifiziert wird</param>
public static void PlaceSpiderWebs(char[,] map)
{
    int height = map.GetLength(0);
    int width = map.GetLength(1);
    Random rnd = new Random();

    // Horizontale Hohlräume prüfen (Zeile für Zeile)
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width - 4; x++)
        {
            // Prüfe ob es ein Hohlraum-Muster gibt: # + 3+ Leerzeichen + #
            if (map[y, x] == '#')
            {
                int spaceCount = 0;
                int spaceStart = x + 1;
                
                // Zähle aufeinanderfolgende freie Kacheln
                while (x + 1 + spaceCount < width && 
                       map[y, x + 1 + spaceCount] == ' ' &&
                       spaceCount < 20)
                {
                    spaceCount++;
                }

                // Genau 3 freie Kacheln und gefolgt von '#', dann Hohlraum
                if (spaceCount == 3 && x + 1 + spaceCount < width && map[y, x + 1 + spaceCount] == '#')
                {
                    // Platziere Spinnenweben zufällig in diesem Hohlraum
                    PlaceWebsInCavity(map, y, spaceStart, spaceCount, rnd);
                }
            }
        }
    }

    // Vertikale Hohlräume prüfen (Spalte für Spalte)
    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height - 4; y++)
        {
            // Prüfe ob es ein Hohlraum-Muster gibt: # + 3+ Leerzeichen + #
            if (map[y, x] == '#')
            {
                int spaceCount = 0;
                int spaceStart = y + 1;
                
                // Zähle aufeinanderfolgende freie Kacheln vertikal
                while (y + 1 + spaceCount < height && 
                       map[y + 1 + spaceCount, x] == ' ' &&
                       spaceCount < 20)
                {
                    spaceCount++;
                }

                // Genau 3 freie Kacheln und gefolgt von '#', dann Hohlraum
                if (spaceCount == 3 && y + 1 + spaceCount < height && map[y + 1 + spaceCount, x] == '#')
                {
                    // Platziere Spinnenweben zufällig in diesem Hohlraum (vertikal)
                    PlaceWebsInCavityVertical(map, x, spaceStart, spaceCount, rnd);
                }
            }
        }
    }
}

/// <summary>
/// Platziert Spinnenweben in einem horizontalen Hohlraum
/// </summary>
private static void PlaceWebsInCavity(char[,] map, int y, int startX, int spaceCount, Random rnd)
{
    // Nur an den Wandkanten platzieren (adjazent zu '#') und selten
    int leftX = startX;                 // direkt rechts von der linken Wand '#'
    int rightX = startX + spaceCount - 1; // direkt links von der rechten Wand '#'

    // Sehr geringe Wahrscheinlichkeit pro Seite
    if (map[y, leftX] == ' ' && rnd.NextDouble() < 0.2)
    {
        map[y, leftX] = 'X';
    }
    if (map[y, rightX] == ' ' && rnd.NextDouble() < 0.2)
    {
        map[y, rightX] = 'X';
    }
}

/// <summary>
/// Platziert Spinnenweben in einem vertikalen Hohlraum
/// </summary>
private static void PlaceWebsInCavityVertical(char[,] map, int x, int startY, int spaceCount, Random rnd)
{
    // Nur an den Wandkanten platzieren (adjazent zu '#') und selten
    int topY = startY;                   // direkt unter der oberen Wand '#'
    int bottomY = startY + spaceCount - 1; // direkt über der unteren Wand '#'

    // Sehr geringe Wahrscheinlichkeit pro Seite
    if (map[topY, x] == ' ' && rnd.NextDouble() < 0.2)
    {
        map[topY, x] = 'X';
    }
    if (map[bottomY, x] == ' ' && rnd.NextDouble() < 0.2)
    {
        map[bottomY, x] = 'X';
    }
}

}