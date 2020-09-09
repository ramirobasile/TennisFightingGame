using System;

namespace TennisFightingGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TennisFightingGame())
                game.Run();
        }
    }
}
