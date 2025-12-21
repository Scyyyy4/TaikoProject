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
    }
}