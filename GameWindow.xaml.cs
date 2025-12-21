using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaikoProject.Core;


namespace TaikoProject
{
    /// <summary>
    /// Main game interface.
    /// A is here:
    /// - Create GameManager
    /// - Establish game loop (timely call Update)
    /// - Handle keyboard input and forward it to GameManager
    /// C will later draw musical notes, scores, etc. in RefreshUI.
    /// </summary>
    public partial class GameWindow : Window
    {
        private GameManager _gameManager;
        private DateTime _lastUpdateTime;

        public GameWindow()
        {
            InitializeComponent();

            // Create and start a game.
            _gameManager = new GameManager();
            _gameManager.StartGame();

            // Initialization time, used later to calculate deltaTime.
            _lastUpdateTime = DateTime.Now;

            // Start the game loop (Call Update periodically).
            StartGameLoop();

            // Enables the window to receive keyboard events.
            this.KeyDown += GameWindow_KeyDown;
            this.Focusable = true;
            this.Focus();
        }

        /// <summary>
        /// Implement a simple game loop using DispatcherTimer.
        /// Call _gameManager.Update and RefreshUI every 16ms (approximately 60 FPS).
        /// </summary>
        private void StartGameLoop()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(16); // Approximately 60 frames
            timer.Tick += (s, e) =>
            {
                var now = DateTime.Now;
                var delta = (now - _lastUpdateTime).TotalSeconds;
                _lastUpdateTime = now;

                // Update game logic (time progression, note status, etc.).
                _gameManager.Update(delta);

                // Refresh the interface based on the latest game status (filled in by C).
                RefreshUI();

                // If GameManager marks the game as over, you can jump to the ResultWindow here.
                if (_gameManager.IsFinished)
                {
                    // TODO：Activate it only when you are ready to present the results:
                    // var w = new ResultWindow();
                    // w.Show();
                    // this.Close();
                }
            };
            timer.Start();
        }

        /// <summary>
        /// Refresh the interface.
        /// C will then update the note positions, score text, judgment text, etc., here based on the data from _gameManager.Notes and ScoreManager.
        /// Leave this blank for now, it will be implemented later.
        /// </summary>
        private void RefreshUI()
        {
            // TODO: Implement the UI update logic in C.
        }

        /// <summary>
        /// Handle keyboard input.
        /// Here, key presses are mapped to red drum / blue drum, and then forwarded to GameManager.
        /// The decision logic is completed in GameManager.ProcessHit.
        /// </summary>
        private void GameWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Let's assign some keys here:
            // D/F are considered the red drum; J/K are considered the blue drum.
            if (e.Key == Key.D || e.Key == Key.F)
            {
                _gameManager.ProcessHit(NoteColor.Red);
            }
            else if (e.Key == Key.J || e.Key == Key.K)
            {
                _gameManager.ProcessHit(NoteColor.Blue);
            }
        }
    }
}
