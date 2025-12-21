using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaikoProject.Core
{
    /// <summary>
    /// 管理分数、连击、各种判定次数。
    /// 具体「加多少分」等细节由 B 决定，这里只提供接口。
    /// </summary>
    public class ScoreManager
    {
        /// <summary>
        /// 当前总分。
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// 当前连击数。
        /// </summary>
        public int Combo { get; private set; }

        /// <summary>
        /// 最大连击（可选，做统计用）。
        /// </summary>
        public int MaxCombo { get; private set; }

        /// <summary>
        /// Perfect 判定次数。
        /// </summary>
        public int PerfectCount { get; private set; }

        /// <summary>
        /// Good 判定次数。
        /// </summary>
        public int GoodCount { get; private set; }

        /// <summary>
        /// Bad 判定次数。
        /// </summary>
        public int BadCount { get; private set; }

        /// <summary>
        /// 处理一次 Perfect 判定。
        /// 在这里可以增加分数、连击等。
        /// 具体加多少由 B 来实现。
        /// </summary>
        public void AddPerfect()
        {
            // TODO: B 来实现具体公式，例如：
            // Score += 300;
            // Combo++;
            // if (Combo > MaxCombo) MaxCombo = Combo;
            // PerfectCount++;
        }

        /// <summary>
        /// 处理一次 Good 判定。
        /// </summary>
        public void AddGood()
        {
            // TODO: B 实现，例如：
            // Score += 100;
            // Combo++;
            // if (Combo > MaxCombo) MaxCombo = Combo;
            // GoodCount++;
        }

        /// <summary>
        /// 处理一次 Bad 判定。
        /// 一般会断连击。
        /// </summary>
        public void AddBad()
        {
            // TODO: B 实现，例如：
            // Combo = 0;
            // BadCount++;
        }
    }
}
