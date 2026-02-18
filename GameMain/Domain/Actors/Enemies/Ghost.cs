using System;
using Microsoft.Xna.Framework;

public class Ghost : Enemy
{
    public override float SightRange { get; protected set; } = 800f;
    public override int XPdrop => 10;
    public override int DamageOnOthers { get; init; } = 25;

    public Ghost()
    {
        EnemyState = new GPatrolState();
        ActorState = new GhostStats();
        Width = Texture.Width;
        Height = Texture.Height;

    }

    public override void Update(GameTime gameTime, Tile[,] map, double levelWeight)
    {
        base.Update(gameTime,map, levelWeight);
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


