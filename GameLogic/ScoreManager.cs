using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaikoProject.Core
{
    /// <summary>
    /// Manages scores, combos, and various judgment counts.
    /// Specific details such as "how many points to add" are determined by B; only the interface is provided here.
    /// </summary>
    public class ScoreManager
    {
        /// <summary>
        /// Current total score.
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Current combo count.
        /// </summary>
        public int Combo { get; private set; }

        /// <summary>
        /// Maximum combo (optional, for statistical purposes).
        /// </summary>
        public int MaxCombo { get; private set; }

        /// <summary>
        /// The number of times "Perfect" is checked.
        /// </summary>
        public int PerfectCount { get; private set; }

        /// <summary>
        /// Number of "Good" judgments.
        /// </summary>
        public int GoodCount { get; private set; }

        /// <summary>
        ///Bad: Number of times it was judged.
        /// </summary>
        public int BadCount { get; private set; }

        /// <summary>
        /// Perform a Perfect check.
        /// Here you can add scores, combos, etc.
        /// The specific amount added is determined by B.
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
        /// Process one Good judgment.
        /// </summary>
        public void AddGood()
        {
            // TODO: Implementation B, for example:
            // Score += 100;
            // Combo++;
            // if (Combo > MaxCombo) MaxCombo = Combo;
            // GoodCount++;
        }

        /// <summary>
        /// Handle a Bad hit.
        /// This usually breaks the combo.
        /// </summary>
        public void AddBad()
        {
            // TODO: Implementation B, for example:
            // Combo = 0;
            // BadCount++;
        }
    }
}
