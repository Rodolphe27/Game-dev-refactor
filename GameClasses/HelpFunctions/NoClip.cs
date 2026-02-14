using Microsoft.Xna.Framework.Input;

public static class NoClip
{
    private static bool isEnabled = false;

    public static bool IsEnabled => isEnabled;

    /// <summary>
    /// Wechselt NoClip an/aus (Standard: N-Taste)
    /// </summary>
    public static void UpdateNoClipToggle()
    {
        if (Controls.Input.IsNewKeyPress(Keys.N))
        {
            isEnabled = !isEnabled;
        }
    }

    /// <summary>
    /// Aktiviert NoClip
    /// </summary>
    public static void Enable()
    {
        isEnabled = true;
    }

    /// <summary>
    /// Deaktiviert NoClip
    /// </summary>
    public static void Disable()
    {
        isEnabled = false;
    }

    /// <summary>
    /// Setzt NoClip zurück
    /// </summary>
    public static void Reset()
    {
        isEnabled = false;
    }
}
