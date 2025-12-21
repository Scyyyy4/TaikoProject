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
    /// 游戏主界面。
    /// A 在这里：
    /// - 创建 GameManager
    /// - 建立游戏循环（定时调用 Update）
    /// - 处理键盘输入，并转发给 GameManager
    /// C 以后会在 RefreshUI 里画音符、分数等。
    /// </summary>
    public partial class GameWindow : Window
    {
        private GameManager _gameManager;
        private DateTime _lastUpdateTime;

        public GameWindow()
        {
            InitializeComponent();

            // 创建并启动一局游戏。
            _gameManager = new GameManager();
            _gameManager.StartGame();

            // 初始化时间，后面用来算 deltaTime。
            _lastUpdateTime = DateTime.Now;

            // 启动游戏循环（定时调用 Update）。
            StartGameLoop();

            // 让窗口可以接收键盘事件。
            this.KeyDown += GameWindow_KeyDown;
            this.Focusable = true;
            this.Focus();
        }

        /// <summary>
        /// 使用 DispatcherTimer 实现一个简单的游戏循环。
        /// 每隔 16ms（约 60 FPS）调用一次 _gameManager.Update 和 RefreshUI。
        /// </summary>
        private void StartGameLoop()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(16); // 约 60 帧
            timer.Tick += (s, e) =>
            {
                var now = DateTime.Now;
                var delta = (now - _lastUpdateTime).TotalSeconds;
                _lastUpdateTime = now;

                // 更新游戏逻辑（时间推进、音符状态等）。
                _gameManager.Update(delta);

                // 根据最新的游戏状态刷新界面（由 C 来填）。
                RefreshUI();

                // 如果 GameManager 标记游戏结束，可以在这里跳到 ResultWindow。
                if (_gameManager.IsFinished)
                {
                    // TODO：等你们准备好展示结果时再启用：
                    // var w = new ResultWindow();
                    // w.Show();
                    // this.Close();
                }
            };
            timer.Start();
        }

        /// <summary>
        /// 刷新界面。
        /// 之后 C 会在这里根据 _gameManager.Notes 和 ScoreManager 的数据，
        /// 更新音符位置、分数文本、判定文本等。
        /// 现在先留空，以后再实现。
        /// </summary>
        private void RefreshUI()
        {
            // TODO: C 实现 UI 更新逻辑。
        }

        /// <summary>
        /// 处理键盘输入。
        /// 这里把按键映射为红鼓 / 蓝鼓，然后转发给 GameManager。
        /// 判定逻辑在 GameManager.ProcessHit 中完成。
        /// </summary>
        private void GameWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // 这里先随便定一个键位：
            // D/F 视为红鼓；J/K 视为蓝鼓。
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
