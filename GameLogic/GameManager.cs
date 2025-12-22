using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaikoProject.Core
{
    
    public class GameManager
    {

        //---NEW ADDED---
        // Timing Windows (seconds)
        public const double PERFECT_WINDOW = 0.08;
        public const double GOOD_WINDOW = 0.15;
        public const double BAD_WINDOW = 0.25;

        /// <summary>
        ///The current game time (in seconds).
        /// This is accumulated by deltaTime passed from A during each Update.
        /// </summary>
        public double CurrentTime { get; private set; } = 0.0;

        /// <summary>
        /// All notes in the current game (single-player mode).
        /// This can be expanded into multiple lists when creating multiple difficulty levels/2P modes in the future.
        /// </summary>
        public List<Note> Notes { get; private set; } = new List<Note>();

        /// <summary>
        /// Scores and judgment statistics.
        /// </summary>
        public ScoreManager ScoreManager { get; private set; } = new ScoreManager();


        /// <summary>
        /// Has the game ended (song finished playing, all notes processed)?
        /// GameWindow will redirect to the Result screen based on this.
        /// </summary>
        public bool IsFinished { get; private set; } = false;

        // ---NEW ADDED---
        // Show last judgement text
        public NoteJudgement LastJudgement { get; private set; } = NoteJudgement.None;

        
        private double _songLengthSeconds = 0.0;
        private const double END_BUFFER_SECONDS = 2.0;


        public GameManager()
        {
            //ScoreManager = new ScoreManager();
            //Notes = new List<Note>();
        }


        // ---NEW ADDED--
        // Optional. If we inject notes from outside
        public void Initialize(IEnumerable<Note> notes)
        {
            Notes = notes.OrderBy(n => n.Time).ToList();

            CurrentTime = 0.0;
            IsFinished = false;
            LastJudgement = NoteJudgement.None;
            ScoreManager.ResetAll();

            foreach (var n in Notes)
            {
                n.IsProcessed = false;
                n.Judgement = NoteJudgement.None;
                n.ProcessedAtTime = null;
            }

            double lastNoteTime = Notes.Count > 0 ? Notes.Max(n => n.Time) : 0.0;
            _songLengthSeconds = lastNoteTime + END_BUFFER_SECONDS;
        }


        /// <summary>
        /// Start a game:
        /// - Reset time
        /// - Reset score
        /// - Load sheet music (currently hardcodes a few notes, can be changed to read from a file later).
        /// </summary>
        public void StartGame()
        {
            // Reset state
            CurrentTime = 0.0;
            IsFinished = false;
            LastJudgement = NoteJudgement.None;
            ScoreManager.ResetAll();


            // simple test chart
            Notes = new List<Note>
            {
                new Note { Time = 1.00, Color = NoteColor.Red },
                new Note { Time = 1.50, Color = NoteColor.Blue },
                new Note { Time = 2.00, Color = NoteColor.Red },
                new Note { Time = 2.50, Color = NoteColor.Blue },
                new Note { Time = 3.00, Color = NoteColor.Red },
                new Note { Time = 3.50, Color = NoteColor.Blue },
            };

            foreach (var n in Notes)
            {
                n.IsProcessed = false;
                n.Judgement = NoteJudgement.None;
                n.ProcessedAtTime = null;
            }

            // Set song length (baseline: last note + buffer)
            double lastNoteTime = Notes.Count > 0 ? Notes.Max(n => n.Time) : 0.0;
            _songLengthSeconds = lastNoteTime + END_BUFFER_SECONDS;

        }

        /// <summary>
        /// Every frame / every time a timer tick is called.
        /// deltaTime: How many seconds have passed in this short period of time (passed by GameWindow).
        /// </summary>
        public void Update(double deltaTime)
        {
            if (IsFinished) return;

            CurrentTime += deltaTime;

            // Missed notes become Bad
            foreach (var note in Notes)
            {
                if (note.IsProcessed) continue;

                // If we passed the note beyond allowed window, then bad
                if (CurrentTime > note.Time + BAD_WINDOW)
                {
                    note.IsProcessed = true;
                    note.Judgement = NoteJudgement.Bad;
                    note.ProcessedAtTime = CurrentTime;

                    ScoreManager.AddBad();
                    LastJudgement = NoteJudgement.Bad;
                }
            }
            // finished when time passed estimated song length AND all notes processed
            if (CurrentTime >= _songLengthSeconds && Notes.All(n => n.IsProcessed))
            {
                IsFinished = true;
            }
        }


        /// <summary>
        /// This is called by GameWindow when the player presses a key (red/blue).
        // This is where the judgment (perfect/good/bad) is made based on CurrentTime and the target note's time.
        /// The specific algorithm is implemented by B.
        /// </summary>
        public void ProcessHit(NoteColor color)
        {
            if (IsFinished) return;

            // 1) Find the closest unprocessed note (any color)
            var closest = Notes
                .Where(n => !n.IsProcessed)
                .OrderBy(n => Math.Abs(CurrentTime - n.Time))
                .FirstOrDefault();

            if (closest == null) return;

            double diff = Math.Abs(CurrentTime - closest.Time);

            // If too far away from any note, ignore keypress
            if (diff > BAD_WINDOW) return;

            // 2) If wrong color: Bad + break combo, and mark note processed
            if (closest.Color != color)
            {
                closest.IsProcessed = true;
                closest.ProcessedAtTime = CurrentTime;
                closest.Judgement = NoteJudgement.Bad;

                ScoreManager.AddBad();
                LastJudgement = NoteJudgement.Bad;
                return;
            }

            // 3) Correct color: judge by timing windows
            closest.IsProcessed = true;
            closest.ProcessedAtTime = CurrentTime;

            if (diff <= PERFECT_WINDOW)
            {
                closest.Judgement = NoteJudgement.Perfect;
                ScoreManager.AddPerfect();
                LastJudgement = NoteJudgement.Perfect;
            }
            else if (diff <= GOOD_WINDOW)
            {
                closest.Judgement = NoteJudgement.Good;
                ScoreManager.AddGood();
                LastJudgement = NoteJudgement.Good;
            }
            else
            {
                closest.Judgement = NoteJudgement.Bad;
                ScoreManager.AddBad();
                LastJudgement = NoteJudgement.Bad;
            }

        }
    }
}

