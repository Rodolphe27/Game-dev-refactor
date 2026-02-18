using Microsoft.Xna.Framework;

public class WChaseState : IEnemyState
{
    private const float GiveUpRange = 100f;
    public void Enter(Enemy enemy)
    {
        enemy.CurrState = Actor.MovStates.Moving;
    }

    public void Exit(Enemy enemy)
    { }

    public void Update(Enemy enemy, Players player, Tile[,] map, double levelWidth)
    {
        float dist = Vector2.Distance(enemy.Pos, player.Pos);
        if(dist < GiveUpRange)
        {
            enemy.SetState(new WAttackState());
            return;
        } 
        enemy.FLyTowards(player.Pos);
}}