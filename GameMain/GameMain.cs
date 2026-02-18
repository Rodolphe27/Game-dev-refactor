using System;
using System.Threading.Tasks.Dataflow;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class GameMain
{
    private ContentManager Content;
    private Camera2D Camera;
    
    private LevelPlayerManager Manager;
    private GUI Gui;
    private Players Player;
    private SpriteFont PixelFont;
    public event Action GameOver;
    public event Action GameWon;
    public GameMain(ContentManager content, Viewport viewport, GraphicsDevice graphicsDevice)
    {
        // Initialise variables for Globals.cs
        Globals.Reset();

        TextureLoader.LoadAll(content, graphicsDevice);
        AudioLoader.LoadAll(content);
        Content = content;
        Player  = new Players();
        Player.OnPlayerDeath += () => GameOver.Invoke();
        Player.OnWin += () => GameWon.Invoke();
        Globals.players = Player;
        Camera  = new(viewport);
        Manager = new(Player);
        PixelFont = Content.Load<SpriteFont>("GUI/GameFont");
        Gui     = new(Player, viewport, PixelFont);

    }
    public void Update(GameTime gameTime, Viewport viewport)
    {
        SetTimer.Update();
        Globals.Update(gameTime);
        Manager.Update(gameTime);
        Gui.Update(viewport);
        Camera.Update(viewport, Manager.LevelHeight, Manager.LevelWidth).Follow(new Vector2(Manager.PlayerX + Manager.PlayerWidth / 2, Manager.PlayerY + Manager.Playerheight / 2));
    } 
    public void Draw(ScreenManager screenManager)
    {
        screenManager.GraphicsDevice.Clear(Color.Black);

        SpriteBatch spriteBatch = screenManager.SpriteBatch;
        spriteBatch.Begin(transformMatrix: Camera.GetTransformation());
        Manager.Draw(spriteBatch);
        spriteBatch.End();

        SpriteBatch spriteBatchGUI = screenManager.SpriteBatch;
        spriteBatchGUI.Begin();
        Gui.Draw(spriteBatchGUI, (IPlayerState)Manager.CurrPlayer().ActorState);
        spriteBatchGUI.End();
    }
}