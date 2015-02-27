using System;
using System.Collections.Generic;
using System.IO;
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
            string path = "score.txt";
            if (!File.Exists(path))
            {
                string createText = Convert.ToString(score);
                File.WriteAllText(path, createText);
                Console.WriteLine("You made the new HIGH SCORE: {0}", score);
            }
            else
            {
                string readText = File.ReadAllText("score.txt");
                if (int.Parse(readText) < score)
                {
                    File.WriteAllText(path, Convert.ToString(score));
                    Console.WriteLine("You made the new HIGH SCORE: {0}", score);
                }
                else
                {
                    Console.WriteLine("High Score: {0}", readText);
                    Console.WriteLine("Your Score: {0}", score);
                }
            }
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
