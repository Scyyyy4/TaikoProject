using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaikoProject.Core
{
    /// <summary>
    /// 管理一局游戏的核心逻辑：
    /// - 谱面（音符列表）
    /// - 当前时间
    /// - 分数管理
    /// - 是否结束
    /// 这里先写框架，具体判定逻辑留给 B 来填。
    /// </summary>
    public class GameManager
    {
        /// <summary>
        /// 当前这局游戏的所有音符（单人模式）。
        /// 以后做多难度 / 2P 时，可以扩展为多个列表。
        /// </summary>
        public List<Note> Notes { get; private set; }

        /// <summary>
        /// 分数与判定统计。
        /// </summary>
        public ScoreManager ScoreManager { get; private set; }

        /// <summary>
        /// 当前游戏进行到的时间（秒）。
        /// 每次 Update 时由 A 这边传入 deltaTime 累加。
        /// </summary>
        public double CurrentTime { get; private set; }

        /// <summary>
        /// 游戏是否结束（歌曲播完、所有音符处理完）。
        /// GameWindow 会根据这个跳转到 Result 界面。
        /// </summary>
        public bool IsFinished { get; private set; }

        public GameManager()
        {
            ScoreManager = new ScoreManager();
            Notes = new List<Note>();
        }

        /// <summary>
        /// 开始一局游戏：
        /// - 重置时间
        /// - 重置分数
        /// - 加载谱面（目前先写死几个音符，之后可以改成从文件读）。
        /// </summary>
        public void StartGame()
        {
            CurrentTime = 0;
            IsFinished = false;
            Notes.Clear();

            // TODO: 这里先硬编码几条音符，方便测试。
            // 例如：2 秒时一个红鼓，3 秒时一个蓝鼓。
            Notes.Add(new Note { Time = 2.0, Color = NoteColor.Red });
            Notes.Add(new Note { Time = 3.0, Color = NoteColor.Blue });
        }

        /// <summary>
        /// 每一帧 / 每一次计时器 tick 调用。
        /// deltaTime：这一小段时间过去了多少秒（由 GameWindow 传入）。
        /// </summary>
        public void Update(double deltaTime)
        {
            if (IsFinished) return;

            // 累加当前时间。
            CurrentTime += deltaTime;

            // TODO: B 在这里根据 CurrentTime 更新音符状态，
            // 判断哪些已经错过，是否标记为 Bad，等等。

            // TODO: 当歌曲结束 + 所有音符都处理完时，把 IsFinished 设为 true。
            // 目前先简单写一个条件，例如时间大于某个值：
            // if (CurrentTime > 10.0) IsFinished = true;
        }

        /// <summary>
        /// 当玩家按下某个键（红 / 蓝）时，由 GameWindow 调用。
        /// 这里根据 CurrentTime 和目标音符时间做判定（perfect / good / bad）。
        /// 具体算法由 B 实现。
        /// </summary>
        public void ProcessHit(NoteColor color)
        {
            if (IsFinished) return;

            // TODO: B 完成：
            // 1. 在 Notes 中找到「最接近当前时间且未被处理」的、同颜色的音符。
            // 2. 计算时间差，根据差距决定 Perfect / Good / Bad。
            // 3. 调用 ScoreManager.AddPerfect / AddGood / AddBad。
            // 4. 标记该音符已被处理（需要在 Note 中加状态字段）。
        }
    }
}

