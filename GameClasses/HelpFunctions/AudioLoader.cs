using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public enum AudioID
{
    Drink, DamageTaken, GameOver, ItemPickUp, Jump, LevelUp, Win, Wings, Shoot, Fireball, Sword
}
public static class AudioLoader
{
    public static Dictionary<AudioID, SoundEffectInstance> Audio = [];
    public static void LoadAll(ContentManager content)
    {
        Audio[AudioID.Drink] = content.Load<SoundEffect>("Audio/Drink").CreateInstance();
        Audio[AudioID.DamageTaken] = content.Load<SoundEffect>("Audio/DamageTaken").CreateInstance();
        Audio[AudioID.GameOver] = content.Load<SoundEffect>("Audio/GameOver").CreateInstance();
        Audio[AudioID.ItemPickUp] = content.Load<SoundEffect>("Audio/ItemPickUp").CreateInstance();
        Audio[AudioID.Jump] = content.Load<SoundEffect>("Audio/Jump").CreateInstance();
        Audio[AudioID.LevelUp] = content.Load<SoundEffect>("Audio/LevelUp").CreateInstance();
        Audio[AudioID.Win] = content.Load<SoundEffect>("Audio/Win").CreateInstance();
        Audio[AudioID.Wings] = content.Load<SoundEffect>("Audio/Wings").CreateInstance();
        Audio[AudioID.Shoot] = content.Load<SoundEffect>("Audio/Shoot").CreateInstance();
        Audio[AudioID.Fireball] = content.Load<SoundEffect>("Audio/Fireball").CreateInstance();
        Audio[AudioID.Sword] = content.Load<SoundEffect>("Audio/Sword").CreateInstance();
    }
}