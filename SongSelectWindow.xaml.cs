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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TaikoProject
{
    /// <summary>
    /// SongSelectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SongSelectWindow : Window
    {
        public SongSelectWindow()
        {
            InitializeComponent();
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            var w = new GameWindow();
            w.Show();
            this.Close();
        }

    }
}
