
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class HealthPotion : Items
{
    public HealthPotion(Vector2 pos) : base(pos) {}
    public override Texture2D Texture => TextureLoader.Texture[TextureID.HealthPotion];
    public override int DamageOnOthers { get; init; } = 0;
    public override void CollectItem(Players player)
    {
        if(player.NumberHealthPotions < 9)
        {
            player.NumberHealthPotions += 1;
            HealthPoints = -1;
            AudioLoader.Audio[AudioID.ItemPickUp].Play();
        }
    }
}