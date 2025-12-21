using System.Windows;
using System.Windows.Input;

namespace TaikoProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Title screen with navigation functionality
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Set focus to start button by default for keyboard navigation
            Loaded += (s, e) => StartButton.Focus();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToSongSelect();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Handle Enter/Space to start game
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                NavigateToSongSelect();
                e.Handled = true;
            }
            // Handle Escape to exit application
            else if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        private void NavigateToSongSelect()
        {
            try
            {
                var songSelectWindow = new SongSelectWindow();
                songSelectWindow.Show();
                this.Close();
            }
            catch (System.Exception ex)
            {
                // Basic error handling for robustness (required by Project.pdf)
                MessageBox.Show($"Failed to load song selection screen: {ex.Message}", 
                               "Error", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Error);
            }
        }
    }
}