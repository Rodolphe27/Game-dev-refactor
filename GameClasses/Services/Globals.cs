
using Microsoft.Xna.Framework;

public class Globals
{
    public const float Gravity = 0.5f;
    public const int TileSize = 72;
    public static int MaxTime = 1000;
    public static int Time = MaxTime;
    public static bool PauseTime = false; // Wenn true, bleibt die Gameplay-Zeit stehen (z.B. TinyTest)
    public static bool TinyTestActive = false; // Overlay-Schalter
    public static double TinyTestRemainingMs = 0; // Restzeit für TinyTest
    public static double elapsedTime = 0.0;
    private static GameTime GameTime;
    public static double TotalTime => GameTime?.TotalGameTime.TotalMilliseconds ?? 0;

    public static Players players;
    public static int EnemyDiedCounter = 0;
    public static int PortalsUsed = 0;

    public static void Reset()
    {
        Time = MaxTime;
        PauseTime = false;
        TinyTestActive = false;
        TinyTestRemainingMs = 0;
        elapsedTime = 0.0;
        EnemyDiedCounter = 0;
        PortalsUsed = 0;
    }
    public static void Update(GameTime gameTime)
    {
        GameTime = gameTime;
        if (PauseTime)
            return; // Gameplay-Zeit anhalten

        elapsedTime += GameTime.ElapsedGameTime.TotalSeconds;
        if(elapsedTime >= 1.0)
        {
            elapsedTime = 0.0;
            Time--;
        }
    }
}