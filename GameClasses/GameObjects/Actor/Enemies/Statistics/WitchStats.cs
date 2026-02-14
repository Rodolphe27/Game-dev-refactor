using System.Collections.Generic;

public class WitchStats : IActorState
{
    private readonly int levelid;

    public WitchStats(int levelid)
    {
        this.levelid = levelid;
        HealthPoints = 50 + (levelid * 10);
        Animations = new Dictionary<Actor.MovStates, Animation>();
        Animations[Actor.MovStates.Default] = new(TextureLoader.TextureList[TextureListID.WitchDefault]);
        Animations[Actor.MovStates.Moving] = new(TextureLoader.TextureList[TextureListID.WitchMoving], frameTime: 0.1);
        Animations[Actor.MovStates.Jumping] = Animations[Actor.MovStates.Moving];
        Animations[Actor.MovStates.Attacking] = Animations[Actor.MovStates.Moving];
    }

    public IActorState.Character Name => IActorState.Character.Witch;

    public float MoveSpeed => 2f + (levelid * 1.5f);
    public float JumpSpeed => 0f;

    public float JumpBoost => 0f;

    public float MaxFallSpeed => 0f;
    public int AttackDamage => 8;

    public int HealthPoints { get; set; }
    public Dictionary<Actor.MovStates, Animation> Animations { get; private set; }

    public Attack[] Attack(Actor actor, float attackrotation)
    {
        return [ new WitchAttack(actor, attackrotation, AttackDamage)];
    }
}