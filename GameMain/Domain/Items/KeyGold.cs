
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class KeyGold : Items
{
    public KeyGold(Vector2 pos) : base(pos) {}
    public override Texture2D Texture => TextureLoader.Texture[TextureID.KeyGolden];
    public override int DamageOnOthers { get; init; } = 0;
    public override void CollectItem(Players player)
    {
        if(player.NumberKeyGold < 9)
        {
            player.NumberKeyGold += 1;
            HealthPoints = -1;
            AudioLoader.Audio[AudioID.ItemPickUp].Play();
        }
    }
}