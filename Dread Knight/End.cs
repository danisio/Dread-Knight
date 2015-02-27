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
            Console.WriteLine("GAME OVER");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
