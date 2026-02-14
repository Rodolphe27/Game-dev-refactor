
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

public class VankoState : IPlayerState
{
    public IActorState.Character Name => IActorState.Character.Vanko;
    public float MoveSpeed => 6.0f;
    public float JumpSpeed => -8.75f;
    public float JumpBoost => -0.75f;
    public float MaxFallSpeed => 9.0f;
    public int DestroyedFireballs = 0;
    public Texture2D HeadTexture => TextureLoader.Texture[TextureID.HeadVanko];
    public Dictionary<Actor.MovStates, Animation> Animations { get; private set; } = [];

    // Specific attributes
    public int HealthPoints { get; set; } = 45;
    public int[] HPMaxLevel { get; set; } = [45, 60, 75];
    public int HPMax => HPMaxLevel[LV];
    public int XP { get; set; } = 0;
    public int XPMax { get; set; } = 100;
    public int LV { get; set; } = 0;
    public int Energy { get; set; } = 60;
    public int[] EnergyMaxLevel { get; set; } = [70, 85, 100];
    public int EnergyMax => EnergyMaxLevel[LV];
    public int[] AttackDamageLevel { get; set; } = [20, 30, 40];
    public int AttackDamage => AttackDamageLevel[LV];
    public int[] NumberFireballsLevel = [1, 3, 5];
    public int NumberFireballs => NumberFireballsLevel[LV];
    public int ShotFireBalls;
    public VankoState()
    {
        Animations[Actor.MovStates.Default]   = new(TextureLoader.TextureList[TextureListID.VankoDefault]);
        Animations[Actor.MovStates.Moving]    = new(TextureLoader.TextureList[TextureListID.VankoMoving], frameTime: 0.035);
        Animations[Actor.MovStates.Jumping]   = new(TextureLoader.TextureList[TextureListID.VankoJumping]);
        Animations[Actor.MovStates.Attacking] = new(TextureLoader.TextureList[TextureListID.VankoAttacking], frameTime: 0.1, true);
    }
    public Attack[] Attack(Actor actor, float attackrotation = 0f)
    {
        if(LV == 0)
        {
            ShotFireBalls = 1;
            return [new FireballAttack(actor, attackrotation + 0f, AttackDamage)];
        }
        else if(LV == 1)
        {
            ShotFireBalls = 3;
            return [
                new FireballAttack(actor, attackrotation + 25f, AttackDamage, 5),
                new FireballAttack(actor, attackrotation + 0f,  AttackDamage),
                new FireballAttack(actor, attackrotation - 25f, AttackDamage, -5),
            ];
        }
        ShotFireBalls = 5;
        return [
            new FireballAttack(actor, attackrotation + 30f, AttackDamage, 10),
            new FireballAttack(actor, attackrotation + 15f, AttackDamage, 5),
            new FireballAttack(actor, attackrotation + 0f,  AttackDamage),
            new FireballAttack(actor, attackrotation - 15f, AttackDamage, -5),
            new FireballAttack(actor, attackrotation - 30f, AttackDamage, -10),
        ];

    }
}