using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace TaikoProject
{
    /// <summary>
    /// SongSelectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SongSelectWindow : Window
    {
        private int _selectedSongIndex = 0;
        private string _selectedDifficulty = "normal";
        private List<SongInfo> _songs;
        private Ellipse[] _selectionIndicators;
        private Button[] _difficultyButtons;

        public SongSelectWindow()
        {
            InitializeComponent();
            InitializeSongs();
            InitializeSelectionIndicators();
            InitializeDifficultyButtons();
            SelectSong(0);
            SelectDifficulty("normal");
        }

        private void InitializeSongs()
        {
            // 硬编码歌曲列表
            _songs = new List<SongInfo>
            {
                new SongInfo { 
                    Title = "Example Song", 
                    Artist = "Artist Name", 
                    FilePath = "example.mp3",
                    ChartPath = "example_chart.txt"
                }
            };
        }

        private void InitializeSelectionIndicators()
        {
            // initialize selection indicators
            _selectionIndicators = new Ellipse[] { SelectedIndicator0 };
        }

        private void InitializeDifficultyButtons()
        {
            // initialize difficulty buttons
            _difficultyButtons = new Button[] { DifficultyEasy, DifficultyNormal, DifficultyHard };
        }

        private void SelectSong(int index)
        {
            if (index < 0 || index >= _selectionIndicators.Length)
                return;

            // update selection state
            _selectedSongIndex = index;
            
            // update UI
            for (int i = 0; i < _selectionIndicators.Length; i++)
            {
                _selectionIndicators[i].Visibility = (i == index) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void SelectDifficulty(string difficulty)
        {
            _selectedDifficulty = difficulty;
            
            // update button style
            foreach (var button in _difficultyButtons)
            {
                if (button.Tag.ToString() == difficulty)
                {
                    button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD15858"));
                    button.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB84747"));
                }
                else
                {
                    button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3E3E40"));
                    button.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD15858"));
                }
            }
        }

        private void SongItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // get the clicked song item
            var border = sender as System.Windows.Controls.Border;
            if (border != null && border.Tag is int index)
            {
                SelectSong(index);
            }
        }

        private void DifficultyButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is string difficulty)
            {
                SelectDifficulty(difficulty);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // go back to main menu
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            // create game window and pass the selected song and difficulty
            var selectedSong = _songs[_selectedSongIndex];
            var gameWindow = new GameWindow(selectedSong, _selectedDifficulty);
            gameWindow.Show();
            this.Close();
        }
    }

    // 歌曲信息类 - 用于存储歌曲数据
    public class SongInfo
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string FilePath { get; set; } // 音频文件路径
        public string ChartPath { get; set; } // 谱面文件路径
    }
}