namespace TennisFightingGame
{
	/// <summary>
	/// Player versus player match.
	/// </summary>
	public class Match : Level
    {
        public Ball ball;
        public Court court;
        public Camera camera;
		public Player[] players;
        public bool inPlay;

		public delegate void MatchEndedEventHandler();
		public delegate void MatchQuittedEventHandler();

		public event MatchEndedEventHandler MatchEnded;
        public event MatchQuittedEventHandler MatchQuitted;

		/// <summary>
		/// Allows derived classes to invoke MatchEnded event.
		/// </summary>
		protected virtual void MatchEnd()
        {
            if (MatchEnded != null)
            {
                MatchEnded.Invoke();
            }
        }

		/// <summary>
		/// Allows derived classes to invoke MatchQuitted event.
		/// </summary>
		protected virtual void MatchQuit()
        {
            if (MatchQuitted != null)
            {
                MatchQuitted.Invoke();
            }
        }
    }
}