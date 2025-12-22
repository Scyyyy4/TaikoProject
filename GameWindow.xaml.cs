using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using TaikoProject.Core;
using System.Windows.Media.Animation;

namespace TaikoProject
{
    /// <summary>
    /// Main game interface.
    /// This class handles:
    /// - Creating GameManager with selected song and difficulty
    /// - Playing background music
    /// - Drawing notes on UI
    /// - Displaying score, combo, and judgments
    /// - Handling keyboard input
    /// </summary>
    public partial class GameWindow : Window
    {
        private GameManager _gameManager;
        private DateTime _lastUpdateTime;
        private MediaPlayer _backgroundMusic;
        private MediaPlayer _redHitSound;
        private MediaPlayer _blueHitSound;
        private readonly SongInfo _selectedSong;
        private readonly string _difficulty;
        private DispatcherTimer _gameTimer;
        private List<Ellipse> _noteVisuals = new List<Ellipse>();
        private const double CANVAS_WIDTH = 800; // Width of the note canvas
        private const double CANVAS_HEIGHT = 100; // Height of the main lane
        private const double HIT_POSITION = 680; // X position where notes should be hit (aligned with drum)
        private const double NOTE_SPEED = 300; // Pixels per second

        public GameWindow(SongInfo selectedSong, string difficulty)
        {
            InitializeComponent();
            _selectedSong = selectedSong;
            _difficulty = difficulty;
            
            // Initialize audio players
            _backgroundMusic = new MediaPlayer();
            _redHitSound = new MediaPlayer();
            _blueHitSound = new MediaPlayer();
            
            // Set up window closing to clean up resources
            this.Closed += GameWindow_Closed;
            
            // Create and start a game with the selected difficulty
            _gameManager = new GameManager();
            _gameManager.StartGame(_selectedSong.ChartPath, _difficulty);
            
            // Update song title display
            SongTitleText.Text = $"Now Playing: {_selectedSong.Title} ({_difficulty.ToUpper()})";
            
            // Initialization time, used later to calculate deltaTime.
            _lastUpdateTime = DateTime.Now;

            // Start the game loop (Call Update periodically).
            StartGameLoop();

            // Load and play background music
            PlayBackgroundMusic();
            
            // Load hit sounds
            LoadHitSounds();
            
            // Enable keyboard input
            this.Focusable = true;
            this.Focus();
        }

        private void GameWindow_Closed(object sender, EventArgs e)
        {
            // Clean up media resources
            _backgroundMusic?.Stop();
            _backgroundMusic?.Close();
            _redHitSound?.Close();
            _blueHitSound?.Close();
            _gameTimer?.Stop();
        }

        private void LoadHitSounds()
        {
            try
            {
                // Load red hit sound
                _redHitSound.Open(new Uri("pack://application:,,,/Resource/red.wav"));
                
                // Load blue hit sound
                _blueHitSound.Open(new Uri("pack://application:,,,/Resource/blue.wav"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load hit sounds: {ex.Message}", "Sound Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void PlayBackgroundMusic()
        {
            try
            {
                // For demo purposes, we use a placeholder path
                // In a real implementation, this would be the actual song file path
                string musicPath = _selectedSong.FilePath;
                
                if (!File.Exists(musicPath))
                {
                    // Use a fallback path if file doesn't exist
                    musicPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "music", "background_music.mp3");
                }
                
                _backgroundMusic.Open(new Uri(musicPath, UriKind.Absolute));
                _backgroundMusic.Volume = 0.7; // Set appropriate volume
                _backgroundMusic.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to play background music: {ex.Message}", "Audio Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Implement a simple game loop using DispatcherTimer.
        /// Call _gameManager.Update and RefreshUI every 16ms (approximately 60 FPS).
        /// </summary>
        private void StartGameLoop()
        {
            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = TimeSpan.FromMilliseconds(16); // Approximately 60 frames
            _gameTimer.Tick += (s, e) =>
            {
                var now = DateTime.Now;
                var delta = (now - _lastUpdateTime).TotalSeconds;
                _lastUpdateTime = now;

                // Update game logic (time progression, note status, etc.).
                _gameManager.Update(delta);

                // Refresh the interface based on the latest game status
                RefreshUI();

                // If GameManager marks the game as over, transition to result screen
                if (_gameManager.IsFinished)
                {
                    _gameTimer.Stop();
                    _backgroundMusic.Stop();
                    
                    // Show result window
                    this.Dispatcher.Invoke(() =>
                    {
                        var resultWindow = new ResultWindow(_gameManager.ScoreManager, _selectedSong.Title);
                        resultWindow.Show();
                        this.Close();
                    });
                }
            };
            _gameTimer.Start();
        }

        /// <summary>
        /// Refresh the interface.
        /// Update note positions, score text, judgment text, etc., based on the data from _gameManager.
        /// </summary>
        private void RefreshUI()
        {
            // Update score and combo displays
            ScoreText.Text = _gameManager.ScoreManager.Score.ToString();
            ComboText.Text = _gameManager.ScoreManager.Combo.ToString();
            
            // Update judgment display (clear after 1 second)
            if (!string.IsNullOrEmpty(_gameManager.LastJudgment))
            {
                Dispatcher.Invoke(() =>
                {
                    JudgmentText.Text = _gameManager.LastJudgment;
                    JudgmentText.Foreground = GetJudgmentColor(_gameManager.LastJudgment);
                    
                    // Clear judgment text after a delay
                    var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                    timer.Tick += (s, e) => 
                    {
                        JudgmentText.Text = "";
                        timer.Stop();
                    };
                    timer.Start();
                });
                
                // Clear the last judgment so it doesn't repeat
                _gameManager.LastJudgment = null;
            }
            
            // Update note positions
            UpdateNotePositions();
        }

        private Brush GetJudgmentColor(string judgment)
        {
            return judgment switch
            {
                "PERFECT" => new SolidColorBrush(Color.FromRgb(255, 217, 83)), // Yellow-gold
                "GOOD" => new SolidColorBrush(Color.FromRgb(89, 192, 222)),   // Blue
                "BAD" => new SolidColorBrush(Color.FromRgb(217, 83, 79)),     // Red
                _ => Brushes.White
            };
        }

        private void UpdateNotePositions()
        {
            // Clear old note visuals
            foreach (var visual in _noteVisuals)
            {
                NoteCanvas.Children.Remove(visual);
            }
            _noteVisuals.Clear();
            
            // Create visuals for active notes
            foreach (var note in _gameManager.Notes)
            {
                if (!note.IsProcessed && note.Time > _gameManager.CurrentTime - 2.0) // Only show notes within 2 seconds
                {
                    // Calculate position based on time difference
                    double timeUntilHit = note.Time - _gameManager.CurrentTime;
                    double xPos = CANVAS_WIDTH - (timeUntilHit * NOTE_SPEED);
                    
                    // Only show notes that are on screen
                    if (xPos > -50 && xPos < CANVAS_WIDTH + 50)
                    {
                        CreateNoteVisual(note, xPos);
                    }
                }
            }
        }

        private void CreateNoteVisual(Note note, double xPos)
        {
            // Create note visual
            var noteVisual = new Ellipse
            {
                Width = 40,
                Height = 40,
                StrokeThickness = 2,
                Stroke = Brushes.White
            };
            
            // Set color based on note type
            if (note.Color == NoteColor.Red)
            {
                noteVisual.Fill = new SolidColorBrush(Color.FromRgb(209, 88, 88)); // Red
            }
            else
            {
                noteVisual.Fill = new SolidColorBrush(Color.FromRgb(71, 184, 184)); // Blue
            }
            
            // Position the note
            Canvas.SetLeft(noteVisual, xPos - 20); // Center the note
            Canvas.SetTop(noteVisual, (CANVAS_HEIGHT / 2) - 20); // Center vertically
            
            // Add to canvas
            NoteCanvas.Children.Add(noteVisual);
            _noteVisuals.Add(noteVisual);
        }

        /// <summary>
        /// Handle keyboard input.
        /// Map key presses to red drum / blue drum, and forward to GameManager.
        /// </summary>
        private void GameWindow_KeyDown(object sender, KeyEventArgs e)
        {
            bool isRedHit = (e.Key == Key.D || e.Key == Key.F);
            bool isBlueHit = (e.Key == Key.J || e.Key == Key.K);
            
            if (isRedHit || isBlueHit)
            {
                // Play appropriate sound effect
                if (isRedHit)
                {
                    _redHitSound.Position = TimeSpan.Zero;
                    _redHitSound.Play();
                }
                else
                {
                    _blueHitSound.Position = TimeSpan.Zero;
                    _blueHitSound.Play();
                }
                
                // Process the hit in game logic
                var color = isRedHit ? NoteColor.Red : NoteColor.Blue;
                _gameManager.ProcessHit(color, _backgroundMusic.Position.TotalSeconds);
            }
        }
    }
}