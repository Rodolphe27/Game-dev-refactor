using System.Collections.Generic;

public class SpiderStats : IActorState
{
    public IActorState.Character Name => IActorState.Character.Spider;
    public float MoveSpeed => 3.0f;
    public float JumpSpeed => -19.0f;
    public float JumpBoost => 20f;
    public float MaxFallSpeed => 12f;
    public int HealthPoints { get; set; } = 30;
    public Dictionary<Actor.MovStates, Animation> Animations { get; private set; } = [];
    public SpiderStats()
    {
        Animations[Actor.MovStates.Default]  = new(TextureLoader.TextureList[TextureListID.SpiderDefault]);
        Animations[Actor.MovStates.Moving]   = new(TextureLoader.TextureList[TextureListID.SpiderMoving], frameTime: 0.1);
        Animations[Actor.MovStates.Jumping]  = Animations[Actor.MovStates.Moving];
        Animations[Actor.MovStates.Attacking]= Animations[Actor.MovStates.Moving];
    }
    public Attack[] Attack(Actor actor, float attackrotation)
    {
        // Spiders don't have actual attacks
        return null;
    }
}