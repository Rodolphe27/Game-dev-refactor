
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

public class RomanState : IPlayerState
{
    public IActorState.Character Name => IActorState.Character.Roman;
    public float MoveSpeed => 5.0f;
    public float JumpSpeed => -10.0f;
    public float JumpBoost => -0.55f;
    public float MaxFallSpeed => 11.0f;
    public float AttackRotation = 0f;
    public bool ShootReady = false;
    public Texture2D HeadTexture => TextureLoader.Texture[TextureID.HeadRoman];
    public Dictionary<Actor.MovStates, Animation> Animations { get; private set; } = [];

    // Specific attributes
    public int HealthPoints { get; set; } = 55;
    public int[] HPMaxLevel { get; set; } = [55, 70, 90];
    public int HPMax => HPMaxLevel[LV];
    public int XP { get; set; } = 0;
    public int XPMax { get; set; } = 100;
    public int LV { get; set; } = 0;
    public int Energy { get; set; } = 60;
    public int[] EnergyMaxLevel { get; set; } = [60, 75, 80];
    public int EnergyMax => EnergyMaxLevel[LV];
    public int[] AttackDamageLevel { get; set; } = [15, 25, 35];
    public int AttackDamage => AttackDamageLevel[LV];
    public RomanState()
    {
        Animations[Actor.MovStates.Default]   = new(TextureLoader.TextureList[TextureListID.RomanDefault]);
        Animations[Actor.MovStates.Moving]    = new(TextureLoader.TextureList[TextureListID.RomanMoving], frameTime: 0.07);
        Animations[Actor.MovStates.Jumping]   = new(TextureLoader.TextureList[TextureListID.RomanJumping]);
        Animations[Actor.MovStates.Attacking] = new(TextureLoader.TextureList[TextureListID.RomanAttacking], frameTime: 0.11, true);
    }
    public Attack[] Attack(Actor actor, float attackrotation = 0f)
    {
        if(LV == 0)
        {
            return [new GunshotAttack(actor, attackrotation, AttackDamage),];
        }
        if(LV == 1)
        {
            return [
                new GunshotAttack(actor, attackrotation - 2.5f, AttackDamage),
                new GunshotAttack(actor, attackrotation + 2.5f, AttackDamage),
            ];
        }
        return [
            new GunshotAttack(actor, attackrotation - 5f, AttackDamage),
            new GunshotAttack(actor, attackrotation, AttackDamage),
            new GunshotAttack(actor, attackrotation + 5f, AttackDamage),
        ];
    }
}