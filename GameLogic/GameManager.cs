using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaikoProject.Core
{
    /// <summary>
    /// Core logic for managing a game:
    /// - Sheet music (note list)
    /// - Current time
    /// - Score management
    /// - Game over status
    /// Here's the framework; the specific decision logic will be filled in by B.
    /// </summary>
    public class GameManager
    {
        /// <summary>
        /// All notes in the current game (single-player mode).
        /// This can be expanded into multiple lists when creating multiple difficulty levels/2P modes in the future.
        /// </summary>
        public List<Note> Notes { get; private set; }

        /// <summary>
        /// Scores and judgment statistics.
        /// </summary>
        public ScoreManager ScoreManager { get; private set; }

        /// <summary>
        ///The current game time (in seconds).
        /// This is accumulated by deltaTime passed from A during each Update.
        /// </summary>
        public double CurrentTime { get; private set; }

        /// <summary>
        /// Has the game ended (song finished playing, all notes processed)?
        /// GameWindow will redirect to the Result screen based on this.
        /// </summary>
        public bool IsFinished { get; private set; }

        public GameManager()
        {
            ScoreManager = new ScoreManager();
            Notes = new List<Note>();
        }

        /// <summary>
        /// Start a game:
        /// - Reset time
        /// - Reset score
        /// - Load sheet music (currently hardcodes a few notes, can be changed to read from a file later).
        /// </summary>
        public void StartGame()
        {
            CurrentTime = 0;
            IsFinished = false;
            Notes.Clear();

            // TODO: Here are some hard-coded notes for easier testing.
            // For example: a red drum at 2 seconds, a blue drum at 3 seconds.
            Notes.Add(new Note { Time = 2.0, Color = NoteColor.Red });
            Notes.Add(new Note { Time = 3.0, Color = NoteColor.Blue });
        }

        /// <summary>
        /// Every frame / every time a timer tick is called.
        /// deltaTime: How many seconds have passed in this short period of time (passed by GameWindow).
        /// </summary>
        public void Update(double deltaTime)
        {
            if (IsFinished) return;

            // Accumulate the current time.
            CurrentTime += deltaTime;

            // TODO: B Update the note status here based on CurrentTime,
            // Determine which notes have been missed, whether to mark them as Bad, etc.
            // TODO: When the song ends and all notes have been processed, set IsFinished to true.
            // For now, let's write a simple condition, for example, if the time is greater than a certain value:
            // if (CurrentTime > 10.0) IsFinished = true;
        }

        /// <summary>
        /// This is called by GameWindow when the player presses a key (red/blue).
        // This is where the judgment (perfect/good/bad) is made based on CurrentTime and the target note's time.
        /// The specific algorithm is implemented by B.
        /// </summary>
        public void ProcessHit(NoteColor color)
        {
            if (IsFinished) return;

            // TODO: B Complete:
            // 1. Find the note of the same color that is closest to the current time and has not been processed in Notes.
            // 2. Calculate the time difference and determine Perfect/Good/Bad based on the difference.
            // 3. Call ScoreManager.AddPerfect/AddGood/AddBad.
            // 4. Mark the note as processed (a status field needs to be added to the Note).
        }
    }
}

