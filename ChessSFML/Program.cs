using System;
using SFML;

namespace ChessSFML
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1024 is the window resolution
            Game game = new Game(1024);

            game.Show();
        }
    }
}
