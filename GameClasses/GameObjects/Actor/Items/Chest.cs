
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class Chest : Items
{
    public Chest(Vector2 pos) : base(pos) {}
    public override Texture2D Texture => TextureLoader.Texture[TextureID.Chest];
    public override int DamageOnOthers { get; init; } = 0;
    public override void CollectItem(Players player)
    {
        var rewards = new List<Action>();

        if (player.NumberHealthPotions < 9)
            rewards.Add(() => player.NumberHealthPotions++);

        if (player.NumberWings < 9)
            rewards.Add(() => player.NumberWings++);

        if (((IPlayerState)player.ActorState).LV < 3)
            rewards.Add(() => ((IPlayerState)player.ActorState).XP += 100);

        if (rewards.Count == 0)
            return;

        Random rng = new Random();
        int index = rng.Next(rewards.Count);
        rewards[index]();

        HealthPoints = -1;
        AudioLoader.Audio[AudioID.ItemPickUp].Play();
    }
}