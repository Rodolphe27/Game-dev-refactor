#region File Description

//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion File Description

namespace GameStateManagement
{
    /// <summary>
    /// The screen comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    internal class GameWonScreen : MenuScreen
    {
        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameWonScreen()
            : base("Congrats, you did it!")
        {
            // Create our menu entries.
            MenuEntry youWongameMenuEntry = new MenuEntry("Back to menu");
            // Hook up menu event handlers.
            youWongameMenuEntry.Selected += ConfirmQuitMessageBoxAccepted;

            // Add entries to the menu.
            MenuEntries.Add(youWongameMenuEntry);
        }

        #endregion Initialization

        #region Handle Input

        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        private void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        private void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
        }

        #endregion Handle Input
    }
}