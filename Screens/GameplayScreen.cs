
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace GameStateManagement
{
    internal class GameplayScreen : GameScreen
    {
        private float pauseAlpha;
        GameMain _gameMain;
        GameTime _gameTime;

        #region Initialization
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }
        public override void LoadContent()
        {
            ScreenManager.Game.ResetElapsedTime();
            GameMain gameMain = new(
                content: new ContentManager(ScreenManager.Game.Services, "Content"),
                viewport: ScreenManager.GraphicsDevice.Viewport,
                graphicsDevice: ScreenManager.GraphicsDevice
            );
            gameMain.GameOver += () =>
            {
                ScreenManager.AddScreen(new GameOverScreen(), ControllingPlayer);
            };
            gameMain.GameWon += () =>
            {
                ScreenManager.AddScreen(new GameWonScreen(), ControllingPlayer);
            };
            _gameMain = gameMain;
        }

        #endregion Initialization

        #region Update and input handling

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            _gameTime = gameTime;

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
            {
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            }
            else
            {
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
            }
        }
        
        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleUserInput(InputState input)
        {
            // The game pauses if the user presses the pause button
            if (input.IsPauseGame(ControllingPlayer))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            _gameMain.Update(_gameTime, ScreenManager.GraphicsDevice.Viewport);
        }

        #endregion Update and input handling

        #region Draw
        public override void Draw(GameTime gameTime)
        {
            _gameMain.Draw(ScreenManager);

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        #endregion Draw
    }
}