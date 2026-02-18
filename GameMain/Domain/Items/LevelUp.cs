
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class LevelUp : Items
{
    public LevelUp(Vector2 pos) : base(pos) {}
    public override Texture2D Texture => TextureLoader.Texture[TextureID.LevelUp];
    public override int DamageOnOthers { get; init; } = 0;
    public override void CollectItem(Players player)
    {
        if(player.GetCurrentPlayerState().LV < 2)
        {
            HealthPoints = -1;
            player.GetCurrentPlayerState().XP += 100;   
            AudioLoader.Audio[AudioID.ItemPickUp].Play();
        }
    }
}