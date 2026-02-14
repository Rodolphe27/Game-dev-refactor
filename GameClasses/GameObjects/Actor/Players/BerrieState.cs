using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

public class BerrieState : IPlayerState
{
    public IActorState.Character Name => IActorState.Character.Berrie;
    public float MoveSpeed => 4.5f;
    public float JumpSpeed => -7.0f;
    public float JumpBoost => -0.6f;
    public float MaxFallSpeed => 13.0f;
    public Texture2D HeadTexture => TextureLoader.Texture[TextureID.HeadBerrie];
    public Dictionary<Actor.MovStates, Animation> Animations { get; private set; } = [];

    // Specific attributes
    public int HealthPoints { get; set; } = 70;
    public int[] HPMaxLevel { get; set; } = [70, 90, 110];
    public int HPMax => HPMaxLevel[LV];
    public int XP { get; set; } = 0;
    public int XPMax { get; set; } = 100;
    public int LV { get; set; } = 0;
    public int Energy { get; set; } = 105;
    public int[] EnergyMaxLevel { get; set; } = [105, 150, 200];
    public int EnergyMax => EnergyMaxLevel[LV];
    public int[] AttackDamageLevel { get; set; } = [35, 50, 65];
    public int AttackDamage => AttackDamageLevel[LV];

    public BerrieState()
    {
        Animations[Actor.MovStates.Default]   = new(TextureLoader.TextureList[TextureListID.BerrieDefault]);
        Animations[Actor.MovStates.Moving]    = new(TextureLoader.TextureList[TextureListID.BerrieMoving], frameTime: 0.055);
        Animations[Actor.MovStates.Jumping]   = new(TextureLoader.TextureList[TextureListID.BerrieJumping]);
        Animations[Actor.MovStates.Attacking] = new(TextureLoader.TextureList[TextureListID.BerrieAttacking], frameTime: 0.0015);
    }
    public Attack[] Attack(Actor actor, float attackrotation = 0f)
    {
        return [new SwordAttack(actor, AttackDamage)];
    }
}