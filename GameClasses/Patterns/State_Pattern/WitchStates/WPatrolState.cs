


using System;
using Microsoft.Xna.Framework;

public class WPatrolState : IEnemyState
{
    private float SinAngle = 0f;
    private float startY =0f;
    private bool initialised = false;
    
    public void Enter(Enemy enemy)
    {
        enemy.CurrState = Actor.MovStates.Default;
        startY = enemy.Pos.Y;
        initialised = true;
        
    }

    public void Exit(Enemy enemy)
    {
        
    }

    public void Update(Enemy enemy, Players player, Tile[,] map, double levelWidth)
    {
        if(!initialised)
        {
            startY = enemy.Pos.Y;
            initialised = true;

        }

        if(enemy.CanSeePlayer(player))
        {
            enemy.SetState(new WChaseState());
            return;
        }

        SinAngle += 0.05f;

        float newY = startY + (float)Math.Sin(SinAngle) * 20f;
        enemy.Pos = new Vector2(enemy.Pos.X, newY);
    }
}

