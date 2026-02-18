using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class PatrolState : IEnemyState
{
    // --- Einstellungen ---
    private int _direction = 1; // 1 = Rechts, -1 = Links
    private float _timer = 0f;
    private const float TurnTimerLimit = 2000f; // Alle 2000ms (2s) Richtung ändern

    public void Enter(Enemy enemy)
    {
        enemy.CurrState = Actor.MovStates.Moving;
    }

    public void Exit(Enemy enemy) { }

    public void Update(Enemy enemy, Players player, Tile[,] map, double levelWidth)
    {
        // 1. KÖRPER: Physik anwenden (nutzt die neue Methode im Enemy)
        enemy.ApplyGravity(map);

        // 2. LOGIK: Sehen wir den Spieler? -> Wechsel zu Chase
        // Wir nutzen die neue Helfer-Methode im Enemy!
        if (enemy.CanSeePlayer(player))
        {
            enemy.SetState(new ChaseState());
            return;
        }

        // 3. BEWEGUNG: Patrouillieren
        PerformPatrol(enemy, map, levelWidth);

        // 4. LOGIK: Zufälliges Umdrehen
        UpdateRandomTurn();
    }

    private void PerformPatrol(Enemy enemy, Tile[,] map, double levelWidth)
    {
        List<Tile> nearbyTiles = enemy.GetNearbySolidTiles(map, 1);
        int moveResult = 0;

        // In die aktuelle Richtung laufen
        if (_direction == 1)
            moveResult = enemy.HandleRight(nearbyTiles, levelWidth);
        else
            moveResult = enemy.HandleLeft(nearbyTiles);

        // Wenn wir gegen eine Wand laufen (Ergebnis 0), drehen wir sofort um
        if (moveResult == 0)
        {
            _direction *= -1;
        }
    }

    private void UpdateRandomTurn()
    {
        // Einfacher Timer (ca. 16ms pro Frame addieren)
        _timer += 16f; 
        if (_timer > TurnTimerLimit)
        {
            _direction *= -1; // Richtung wechseln
            _timer = 0;
        }
    }
}