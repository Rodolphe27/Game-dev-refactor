using Microsoft.Xna.Framework.Input;

/// <summary>
/// Handles keyboard input and allows detection of new key presses.
/// </summary>
public class InputHandler
{
    private KeyboardState CurrentKeyboardState;
    private KeyboardState PreviousKeyboardState;
    public void Update()
    {
        PreviousKeyboardState = CurrentKeyboardState;
        CurrentKeyboardState = Keyboard.GetState();
    }
    public bool IsNewKeyPress(Keys key)
    {
        return CurrentKeyboardState.IsKeyDown(key) &&
               !PreviousKeyboardState.IsKeyDown(key);
    }
    public bool IsKeyHeld(Keys key)
    {
        return CurrentKeyboardState.IsKeyDown(key);
    }
    public bool IsKeyReleased(Keys key)
    {
        return !CurrentKeyboardState.IsKeyDown(key) &&
                PreviousKeyboardState.IsKeyDown(key);
    }
}