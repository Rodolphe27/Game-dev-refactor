using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GUI
{
    private Players Player;
    private Vector2 PosBackground;
    private Vector2 PosTimer;
    private Texture2D Texture = TextureLoader.Texture[TextureID.StatsBackground];
    private float Width => Texture.Width;
    private float Height => Texture.Height;
    private Vector2 Origin;
    private SpriteFont Font;
    public GUI(Players player, Viewport viewport, SpriteFont font)
    {
        Player = player;
        Origin = new(0, Height);
        PosBackground = new(0, viewport.Height);
        Font = font;
    }
    public void Update(Viewport viewport)
    {
        PosBackground = new(0, viewport.Height);
        PosTimer = new(10, 10);
    }
    public void Draw(SpriteBatch spriteBatch, IPlayerState playerState)
    {
        #region Timer

        spriteBatch.DrawString(Font, $"{Globals.Time} / {Globals.MaxTime}", PosTimer, Color.White, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, 1f);

        // TinyTest Overlay: Countdown und Hinweis
        if (Globals.TinyTestActive)
        {
            double remainingSec = Math.Max(0, Globals.TinyTestRemainingMs) / 1000.0;
            string line1 = "Training: TinyTest";
            string line2 = $"Zeit verbleibend: {Math.Ceiling(remainingSec)}s";
            Vector2 pos = PosTimer + new Vector2(0, 24);
            spriteBatch.DrawString(Font, line1, pos, Color.LightGreen, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(Font, line2, pos + new Vector2(0, 18), Color.LightGreen, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1f);
        }

        #endregion

        #region Stats

        spriteBatch.Draw(Texture, PosBackground, null, Color.White, 0f, Origin, 0.5f, SpriteEffects.None, 0f);

        Vector2 textPos = PosBackground + new Vector2(0, -10);
        int offsetX = 12;
        int spacingY = 18;

        // Player Head
        Texture2D Head = playerState.HeadTexture;
        int headOffsetX = playerState.XP switch
        {
            < 10 => 0,
            < 100 => 10,
            _ => 20
        };
        Vector2 HeadOrigin = new(Head.Width / 2, Head.Height / 2);
        spriteBatch.Draw(Head, textPos + new Vector2(168 + headOffsetX, -72), null, Color.White, 0f, HeadOrigin, 1f, SpriteEffects.None, 0f);

        // Item count
        int offsetItemY = 130;
        int spacingItemY = 28;
        spriteBatch.Draw(TextureLoader.Texture[TextureID.HealthPotion], textPos + new Vector2(10, -offsetItemY), null, Color.White, 0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);
        spriteBatch.Draw(TextureLoader.Texture[TextureID.Wings], textPos + new Vector2(10, -offsetItemY - spacingItemY), null, Color.White, 0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);
        spriteBatch.Draw(TextureLoader.Texture[TextureID.KeySilver], textPos + new Vector2(10, -offsetItemY - spacingItemY * 2), null, Color.White, 0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);
        spriteBatch.Draw(TextureLoader.Texture[TextureID.KeyGolden], textPos + new Vector2(10, -offsetItemY - spacingItemY * 3), null, Color.White, 0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);

        spriteBatch.DrawString(Font, $"x{Player.NumberHealthPotions}", textPos + new Vector2(offsetX + 24, -spacingY * 7), Color.White);
        spriteBatch.DrawString(Font, $"x{Player.NumberWings}", textPos + new Vector2(offsetX + 24, -spacingY * 7 - spacingItemY), Color.White);
        spriteBatch.DrawString(Font, $"x{Player.NumberKeySilver}", textPos + new Vector2(offsetX + 24, -spacingY * 7 - spacingItemY * 2), Color.White);
        spriteBatch.DrawString(Font, $"x{Player.NumberKeyGold}", textPos + new Vector2(offsetX + 24, -spacingY * 7 - spacingItemY * 3), Color.White);

        // Current Player Stats
        spriteBatch.DrawString(Font, $"HP: {playerState.HealthPoints}/{playerState.HPMax}",             textPos + new Vector2(offsetX, -spacingY * 5), Color.White);
        spriteBatch.DrawString(Font, $"XP: {playerState.XP}/{playerState.XPMax}",             textPos + new Vector2(offsetX, -spacingY * 4), Color.White);
        spriteBatch.DrawString(Font, $"LV: {playerState.LV}/2",                               textPos + new Vector2(offsetX, -spacingY * 3), Color.White);
        spriteBatch.DrawString(Font, $"Energy: {playerState.Energy}/{playerState.EnergyMax}", textPos + new Vector2(offsetX, -spacingY * 2), Color.White);
        spriteBatch.DrawString(Font, $"Attack dmg: {playerState.AttackDamage}",               textPos + new Vector2(offsetX, -spacingY), Color.White);

        #endregion
    }
}