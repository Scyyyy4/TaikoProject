using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaikoProject.Core
{
    /// <summary>
    /// Manages scores, combos, and various judgment counts.
    /// </summary>
    public class ScoreManager
    {
        /// <summary>
        /// Current total score.
        /// </summary>
        public int Score { get; private set; } = 0;

        /// <summary>
        /// Current combo count.
        /// </summary>
        public int Combo { get; private set; } = 0;

        /// <summary>
        /// Maximum combo (optional, for statistical purposes).
        /// </summary>
        public int MaxCombo { get; private set; } = 0;

        /// <summary>
        /// The number of times "Perfect" is checked.
        /// </summary>
        public int PerfectCount { get; private set; } = 0;

        /// <summary>
        /// Number of "Good" judgments.
        /// </summary>
        public int GoodCount { get; private set; } = 0;

        /// <summary>
        ///Bad: Number of times it was judged.
        /// </summary>
        public int BadCount { get; private set; } = 0;
      
        // ---NEW ADDED---
        // Values of perfect, good, bad
        private const int PERFECT_SCORE = 300;
        private const int GOOD_SCORE = 100;
        private const int BAD_SCORE = 0;

        public void AddPerfect()
        {
            PerfectCount++;
            IncreaseCombo();
            Score += PERFECT_SCORE * GetComboMultiplier();
        }

        /// <summary>
        /// Process one Good judgment.
        /// </summary>
        public void AddGood()
        {
            GoodCount++;
            IncreaseCombo();
            Score += GOOD_SCORE * GetComboMultiplier();
        }

        /// <summary>
        /// Handle a Bad hit.
        /// This usually breaks the combo.
        /// </summary>
        public void AddBad()
        {
            BadCount++;
            Score += BAD_SCORE;
            ResetCombo();
        }

        //---NEW ADDED---
        // Reset game state.
        public void ResetAll()
        {
            Score = 0;
            Combo = 0;
            MaxCombo = 0;

            PerfectCount = 0;
            GoodCount = 0;
            BadCount = 0;
        }

        //---NEW ADDED---
        // if combo >=5, score doubled
        // if >=10, score tripled
        private int GetComboMultiplier()
        {
            if (Combo >= 10) return 3;
            if (Combo >= 5) return 2;
            return 1;
        }


        //---NEW ADDED---
        // Increase combo number.
        // Handle final MaxCombo value.
        private void IncreaseCombo()
        {
            Combo++;
            if (Combo > MaxCombo) MaxCombo = Combo;
        }

        //---NEW ADDED---
        // Reset combo.
        private void ResetCombo()
        {
            Combo = 0;
        }
    }
}
