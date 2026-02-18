
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class Wings : Items
{
    public Wings(Vector2 pos) : base(pos) {}
    public override Texture2D Texture => TextureLoader.Texture[TextureID.Wings];
    public override int DamageOnOthers { get; init; } = 0;
    public override void CollectItem(Players player)
    {
        if(player.NumberWings < 9)
        {
            player.NumberWings += 1;
            HealthPoints = -1;
            AudioLoader.Audio[AudioID.ItemPickUp].Play();
        }
    }
}