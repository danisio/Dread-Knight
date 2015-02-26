using System;
using Dread_Knight;

namespace Dread_Knight
{
    struct Object
    {
        public int x;
        public int y;
        public string s;
        public ConsoleColor color;
    }

    class Program
    {
        static void Main()
        {
            Console.BufferHeight = Console.WindowHeight = 35;
            Console.BufferWidth = Console.WindowWidth = 125;

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Black;

            Animation.FirstStage();

            int numberOfPlayers = Intro.Menu();

            if (numberOfPlayers == 1)
                MultyPlayer.MultyPlay();
            else
            {
                MultyPlayer.MultyPlay(true);
            }
        }
    }
}