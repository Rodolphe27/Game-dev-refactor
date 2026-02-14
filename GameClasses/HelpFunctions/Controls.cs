
using Microsoft.Xna.Framework.Input;

public static class Controls
{
    public static InputHandler Input = new();
    public static bool PressLeft => Input.IsKeyHeld(Keys.Left) || Input.IsKeyHeld(Keys.A);
    public static bool PressRight => Input.IsKeyHeld(Keys.Right) || Input.IsKeyHeld(Keys.D);
    public static bool NewPressCharacterSwitch => Input.IsNewKeyPress(Keys.LeftShift);
    public static bool NewPressAttack => Input.IsNewKeyPress(Keys.Enter);
    public static bool PressAttack => Input.IsKeyHeld(Keys.Enter);
    public static bool PressSwitch => Input.IsNewKeyPress(Keys.LeftShift) || Input.IsNewKeyPress(Keys.RightShift);
    public static bool NewPressUp => Input.IsNewKeyPress(Keys.Up) || Input.IsNewKeyPress(Keys.W) || Input.IsNewKeyPress(Keys.Space);
    public static bool PressUp => Input.IsKeyHeld(Keys.Up) || Input.IsKeyHeld(Keys.W) || Input.IsKeyHeld(Keys.Space);
    public static bool PressDown => Input.IsKeyHeld(Keys.Down) || Input.IsKeyHeld(Keys.S);
    public static bool PressDrinkHP => Input.IsNewKeyPress(Keys.H);
    public static bool PressUseWings => Input.IsNewKeyPress(Keys.J);
}