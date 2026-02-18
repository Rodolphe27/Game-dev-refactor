
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class KeySilver : Items
{
    public KeySilver(Vector2 pos) : base(pos) {}
    public override Texture2D Texture => TextureLoader.Texture[TextureID.KeySilver];
    public override int DamageOnOthers { get; init; } = 0;
    public override void CollectItem(Players player)
    {
        if(player.NumberKeySilver < 9)
        {
            player.NumberKeySilver += 1;
            HealthPoints = -1;
            AudioLoader.Audio[AudioID.ItemPickUp].Play();
        }
    }
}