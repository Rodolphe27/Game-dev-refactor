using System;
using Microsoft.Xna.Framework;

public class Witch : Enemy
{
    public override float SightRange { get; protected set; } 
    public Witch(int levelid)
    {

        
        DamageOnOthers = 20 + levelid*10;
        SightRange = 600f + (levelid * 50f);
        EnemyState = new WPatrolState();
        ActorState = new WitchStats(levelid);
        Width = Texture.Width;
        Height = Texture.Height;
    }
    public override int XPdrop => 50;

    public override int DamageOnOthers { get; init;}
    public override void Update(GameTime gameTime, Tile[,] map, double levelWidth)
    {
        base.Update(gameTime, map, levelWidth);
    }

    public override Items OnDeath(Players player)
    {
        IPlayerState state = (IPlayerState)player.ActorState;
        state.XP += XPdrop;

        return new KeyGold(Pos);
    }
}