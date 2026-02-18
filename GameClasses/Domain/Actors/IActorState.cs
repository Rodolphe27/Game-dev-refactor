
using System.Collections.Generic;

public interface IActorState
{
    public enum Character
    {
        Berrie,
        Roman,
        Vanko,
        Spider,
        Ghost,
        Witch
    }
    Character Name { get; }
    float MoveSpeed { get; }
    float JumpSpeed { get; }
    float JumpBoost { get; }
    float MaxFallSpeed { get; }
    int HealthPoints { get; set; }
    Dictionary<Actor.MovStates, Animation> Animations { get;}
    Attack[] Attack(Actor actor, float attackrotation);
}