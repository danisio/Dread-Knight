using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dread_Knight
{
    class End
    {
        internal static void GameOver(int score)
        {
            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth - 9) / 2, Console.WindowHeight / 2);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("GAME OVER");
            for (int i = 1; i < 7; i++)
            {
                Console.Beep(498 - i * 64, 300);
            }
            Console.Beep(70, 1000);

            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
