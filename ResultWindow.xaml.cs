using System;
using System.Windows;
using System.Windows.Media;
using TaikoProject.Core;

namespace TaikoProject
{
    /// <summary>
    /// ResultWindow.xaml 的交互逻辑
    /// Displays game results including score, combo, and judgement counts
    /// </summary>
    public partial class ResultWindow : Window
    {
        private readonly ScoreManager _scoreManager;
        private readonly string _songName;

        public ResultWindow(ScoreManager scoreManager, string songName)
        {
            InitializeComponent();
            _scoreManager = scoreManager;
            _songName = songName;
            
            // Load results into the UI
            LoadResults();
        }

        private void LoadResults()
        {
            // Set song name
            SongNameText.Text = $"SONG: {_songName.ToUpper()}";
            
            // Set score
            ScoreText.Text = _scoreManager.Score.ToString();
            
            // Set max combo
            MaxComboText.Text = _scoreManager.MaxCombo.ToString();
            
            // Set judgement counts
            PerfectCountText.Text = _scoreManager.PerfectCount.ToString();
            GoodCountText.Text = _scoreManager.GoodCount.ToString();
            BadCountText.Text = _scoreManager.BadCount.ToString();
            
            // Calculate and set accuracy
            double accuracy = CalculateAccuracy();
            AccuracyText.Text = $"{accuracy:F2}%";
            
            // Set accuracy color based on value
            if (accuracy >= 90) 
                AccuracyText.Foreground = new SolidColorBrush(Color.FromRgb(139, 195, 74)); // Green
            else if (accuracy >= 80)
                AccuracyText.Foreground = new SolidColorBrush(Color.FromRgb(255, 217, 83)); // Yellow
            else 
                AccuracyText.Foreground = new SolidColorBrush(Color.FromRgb(217, 83, 79)); // Red
        }

        private double CalculateAccuracy()
        {
            int totalHits = _scoreManager.PerfectCount + _scoreManager.GoodCount + _scoreManager.BadCount;
            if (totalHits == 0) return 100.0;
            
            // Perfect counts as 100%, Good as 50%, Bad as 0%
            double weightedScore = 
                (_scoreManager.PerfectCount * 1.0) + 
                (_scoreManager.GoodCount * 0.5);
            
            return (weightedScore / totalHits) * 100.0;
        }

        private void RetryButton_Click(object sender, RoutedEventArgs e)
        {
            // Start a new game with the same song and settings
            // For now, we'll just go back to song selection
            var songSelectWindow = new SongSelectWindow();
            songSelectWindow.Show();
            this.Close();
        }

        private void BackToSongSelectButton_Click(object sender, RoutedEventArgs e)
        {
            // Return to song selection screen
            var songSelectWindow = new SongSelectWindow();
            songSelectWindow.Show();
            this.Close();
        }
    }
}