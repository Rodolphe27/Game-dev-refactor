using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class ChaseState : IEnemyState
{
    // --- Einstellungen ---
    private const float GiveUpRange = 400f; // Ab hier gibt er auf
     // Wenn Spieler 1 Block höher ist

    public void Enter(Enemy enemy)
    {
        // Optional: Sound abspielen oder rot werden
        enemy.CurrState = Actor.MovStates.Moving;
    }

    public void Exit(Enemy enemy) { }

    public void Update(Enemy enemy, Players player, Tile[,] map, double levelWidth)
    {
        // 1. KÖRPER: Physik muss immer laufen
        enemy.ApplyGravity(map);

        // 2. LOGIK: Ist der Spieler zu weit weg? -> Zurück zu Patrol
        float dist = Vector2.Distance(enemy.Pos, player.Pos);
        if (dist > GiveUpRange)
        {
            enemy.SetState(new PatrolState());
            return;
        }

        // 3. BEWEGUNG: Verfolgung & Springen
        PerformChase(enemy, player, map, levelWidth);
    }

    private void PerformChase(Enemy enemy, Players player, Tile[,] map, double levelWidth)
    {
        List<Tile> nearbyTiles = enemy.GetNearbySolidTiles(map, 1);
        
        // Richtung zum Spieler bestimmen
        float diffX = player.Pos.X - enemy.Pos.X;
        int moveResult = 0;

        // Laufen
        if (diffX > 0)
            moveResult = enemy.HandleRight(nearbyTiles, levelWidth);
        else
            moveResult = enemy.HandleLeft(nearbyTiles);

        // --- Sprung-KI ---
        // Wir nutzen die sauberen Properties vom Enemy (IsGrounded)
        
        // Grund A: Wir sind gegen eine Wand gelaufen
        bool hitWall = (moveResult == 0);
        
        // Grund B: Der Spieler steht auf einer Plattform über uns
        float diffY = player.Pos.Y - enemy.Pos.Y;

        // Wenn wir am Boden sind UND (Wand im Weg ODER Spieler ist oben)
        if (enemy.IsGrounded && hitWall)
        {
            enemy.Jump(); // Befehl an den Körper: "Spring!"
        }
    }
}