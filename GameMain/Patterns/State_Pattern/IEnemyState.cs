public interface IEnemyState
{
    void Enter(Enemy enemy);
    void Update(Enemy enemy,Players player, Tile[,] map, double levelWidth);
    void Exit(Enemy enemy);
}