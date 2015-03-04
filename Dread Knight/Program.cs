using System;
using Dread_Knight;
using System.Media;
using System.IO;

namespace Dread_Knight
{
    class Program
    {
        static void Main()
        {
            SoundPlayer sound = new SoundPlayer();
            try
            {
                sound = new SoundPlayer("../../intro.wav");
                sound.PlayLooping();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File intro.wav not found");
            }

            Console.BufferHeight = Console.WindowHeight = 35;
            Console.BufferWidth = Console.WindowWidth = 125;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Title = "KILLING NINJAS";
            Animation.FirstStage();

            int numberOfPlayers = Intro.Menu();
            sound.Stop();

            if (numberOfPlayers == 1)
            {
                
                MultyPlayer.MultyPlay();
            }
            else
            {
                MultyPlayer.MultyPlay(true);
            }
        }
    }
}
