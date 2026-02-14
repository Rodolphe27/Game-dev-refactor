using System;
using Microsoft.Xna.Framework;
using static Actor;

public class GAttackState : IEnemyState
{
    private float cooldownTimer =0f;
    private float fireRate = 2.0f;


    public void Enter(Enemy enemy)
    {
        enemy.CurrState = MovStates.Attacking;
        cooldownTimer = 0.5f;

    }

    public void Exit(Enemy enemy)
    {
        
    }

    public void Update(Enemy enemy, Players player, Tile[,] map, double levelWidth)
    {
    
        if(Vector2.Distance(enemy.Pos, player.Pos)> 300f)
        {
            enemy.SetState(new GPatrolState());
            return;
            
        }

        cooldownTimer -= 0.3f;

        enemy.Direction = (enemy.Pos.X>player.Pos.X) ? Directions.Right : Directions.Left;

        if(cooldownTimer<=0)
            {
                Shoot(enemy,player);
                cooldownTimer = fireRate;
            }

    
    }
    private void Shoot(Enemy enemy, Players player)
    {
        Vector2 dir = player.Pos -enemy.Pos;
        float angle = MathF.Atan2(dir.Y, dir.X);
        float degree = MathHelper.ToDegrees(angle);
        if(enemy.ActorState is GhostStats stats)
        {
            Attack[] attacks = stats.Attack(enemy, degree);
            if (attacks != null && attacks.Length>0)
            {

                enemy.SpawnAttack(attacks);

            }
        }

}}