using System;
using Microsoft.Xna.Framework;
using static Actor;

public class WAttackState : IEnemyState
{
    private float _cooldownTimer = 0f;
    private float _fireRate = 1.5f; // Witches often fire slightly faster or differently than ghosts
    private const float AttackRange = 3000f; // Adjusted range for Witch

    public void Enter(Enemy enemy)
    {
        enemy.CurrState = MovStates.Attacking;
        _cooldownTimer = 0.5f; // Slight delay before first shot
    }

    public void Exit(Enemy enemy)
    {
        // Reset any witch-specific logic here if needed
    }

    // Remember: Update signature must match the IEnemyState interface (including GameTime)
    

    private void Shoot(Enemy enemy, Players player)
    {
        Vector2 dir = player.Pos - enemy.Pos;
        float angle = MathF.Atan2(dir.Y, dir.X);
        float degree = MathHelper.ToDegrees(angle);

        // Cast to WitchStats instead of GhostStats
        if (enemy.ActorState is WitchStats stats)
        {
            Attack[] attacks = stats.Attack(enemy, degree);
            
            if (attacks != null && attacks.Length > 0)
            {
                enemy.SpawnAttack(attacks);
            }
        }
    }

    public void Update(Enemy enemy, Players player, Tile[,] map, double levelWidth)
    {
        // 1. Optimization: Distance Squared check
        float distSq = Vector2.DistanceSquared(enemy.Pos, player.Pos);

        if (distSq > (AttackRange * AttackRange))
        {
            // Switch to the Witch's Patrol state
            enemy.SetState(new WChaseState());
            return;
        }

        // 2. Logic: Face the player
        // If Witch X > Player X, Witch is on the Right, so face Left.
        enemy.Direction = (enemy.Pos.X > player.Pos.X) ? Directions.Right : Directions.Left;

        // 3. Logic: Frame-rate independent timer
        _cooldownTimer -= (float)0.3f;;

        // Always move towards player while attacking
        enemy.FLyTowards(player.Pos);

        if (_cooldownTimer <= 0)
        {
            Shoot(enemy, player);
            _cooldownTimer = _fireRate;
        }
    }
}