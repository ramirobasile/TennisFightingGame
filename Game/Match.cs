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

		public delegate void EndedEventHandler();
		public delegate void QuittedEventHandler();

		public event EndedEventHandler Ended;
        public event QuittedEventHandler Quitted;

		/// <summary>
		/// Allows derived classes to invoke MatchEnded event.
		/// </summary>
		protected virtual void End(Player winner)
        {
            if (Ended != null)
            {
                Ended.Invoke();
            }
        }

		/// <summary>
		/// Allows derived classes to invoke MatchQuitted event.
		/// </summary>
		protected virtual void Quit()
        {
            if (Quitted != null)
            {
                Quitted.Invoke();
            }
        }
    }
}