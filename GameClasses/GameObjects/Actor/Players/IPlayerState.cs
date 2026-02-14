
using Microsoft.Xna.Framework.Graphics;

public interface IPlayerState : IActorState
{
    Texture2D HeadTexture { get; }
    int HPMax { get; }
    int XP { get; set; }
    int XPMax { get; set; }
    int LV { get; set; }
    int Energy { get; set; }
    int EnergyMax { get; }
    int AttackDamage { get; }
}