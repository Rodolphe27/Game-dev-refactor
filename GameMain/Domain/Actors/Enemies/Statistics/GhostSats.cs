using System.Collections.Generic;

public  class GhostStats : IActorState
{
    public IActorState.Character Name => IActorState.Character.Ghost;

    public float MoveSpeed => 2.5f;

    public float JumpSpeed => 0f;

    public float JumpBoost => 0f;

    public float MaxFallSpeed => 0f;
    public int HealthPoints { get; set; } = 40;
    public int AttackDamage => 10;

    public Dictionary<Actor.MovStates, Animation> Animations { get; set; } = [];

    public GhostStats()
    {
        Animations[Actor.MovStates.Default] = new(TextureLoader.TextureList[TextureListID.GhostDefault]);
        Animations[Actor.MovStates.Moving] = new(TextureLoader.TextureList[TextureListID.GhostMoving], frameTime: 0.1);
        Animations[Actor.MovStates.Jumping] = Animations[Actor.MovStates.Moving];
        Animations[Actor.MovStates.Attacking] = Animations[Actor.MovStates.Moving];
    }

    public Attack[] Attack(Actor actor, float attackrotation)
    {
        return [ new GhostFireballAttack(actor, attackrotation, AttackDamage)];
    }
}