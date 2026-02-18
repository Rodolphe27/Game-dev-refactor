using System;
using Microsoft.Xna.Framework;

public class Spider : Enemy
{
    public override int XPdrop => 10;
    public override int DamageOnOthers { get; init; } = 25;
    public Spider()
    {
        EnemyState = new PatrolState();
        ActorState = new SpiderStats();
        Width = Texture.Width;
        Height = Texture.Height;
    }
    public override void Update(GameTime gameTime, Tile[,] map, double levelWidth)
    {
        base.Update(gameTime, map, levelWidth);
    }

    public override Items OnDeath(Players player)
    {
        IPlayerState state = (IPlayerState)player.ActorState;
        state.XP += XPdrop;

        double value = new Random().NextDouble();

        if(value <= 0.35)
        {
            return new HealthPotion(Pos);
        }
        if(value <= 0.55)
        {
            return new Wings(Pos);
        }
        if(value <= 0.6)
        {
            return new KeySilver(Pos);
        }
        else return null;
    }
}