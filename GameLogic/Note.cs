using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaikoProject.Core
{
    /// <summary>
    /// The note color is currently set to red or blue, with more options to be added later.
    /// </summary>
    public enum NoteColor
    {
        Red,
        Blue
    }

    // ---ADDED---
    // Judgment result for a note.
    public enum NoteJudgement
    {
        None,
        Perfect,
        Good,
        Bad
    }

    /// <summary>
    /// Represents a note on a musical score.
    // Currently only includes: appearance time + color.
    // In the future, we can add: status (whether it has been hit), position, etc.
    /// </summary>
    public class Note
    {
        /// <summary>
        /// The time (in seconds, from the start of the song) at which this note should be hit.
        /// B will use this time to compare with the current time to determine Perfect / Good / Bad.
        /// </summary>
        public double Time { get; set; }


        /// <summary>
        /// Is this note from a red drum or a blue drum?
        /// In UI design, you can also select different images based on color.
        /// </summary>
        public NoteColor Color { get; set; }


        //---NEW ADDED---
        // Whether this note has already been handled
        // Used for prevention of double-scoring
        public bool IsProcessed { get; set; } = false;
        //---NEW ADDED---
        // The final judgement assigned to this note
        public NoteJudgement Judgement { get; set; } = NoteJudgement.None;

        //---NEW ADDED---
        //Actual time the note was processed
        //For Debugging
        public double? ProcessedAtTime { get; set; } = null;
    }
}