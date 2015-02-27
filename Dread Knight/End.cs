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
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.SetCursorPosition(0,0);
            string gameOver = @"
                                            ███████      ███      ███   ███  ███████
                                            ██          ██ ██     ██ █ █ ██  ██      
                                            ██  ███    ██   ██    ██  █  ██  ██████  
                                            ██   ██   ██ ███ ██   ██     ██  ██      
                                            ███████  ██       ██  ██     ██  ███████
                                                                                    
                                                                                    
                                            ███████  ██       ██  ███████  ███████
                                            ██   ██   ██     ██   ██       ██   ██
                                            ██   ██    ██   ██    ██████   ███████
                                            ██   ██     ██ ██     ██       ██ ██   
                                            ███████      ███      ███████  ██   ██ 
";
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(gameOver);

            for (int i = 1; i < 7; i++)
            {
                Console.Beep(498 - i * 64, 300);
            }
            Console.Beep(70, 1000);
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
            string path = "score.txt";
            if (!File.Exists(path))
            {
                string createText = Convert.ToString(score);
                File.WriteAllText(path, createText);
                Console.SetCursorPosition((Console.WindowWidth - "You made the new HIGH SCORE: {0}".Length) / 2, Console.WindowHeight / 2 + 1);
                Console.WriteLine("You made the new HIGH SCORE: {0}", score);
            }
            else
            {
                string readText = File.ReadAllText("score.txt");
                if (int.Parse(readText) < score)
                {
                    File.WriteAllText(path, Convert.ToString(score));
                    Console.SetCursorPosition((Console.WindowWidth - "You made the new HIGH SCORE: {0}".Length) / 2, Console.WindowHeight / 2 + 1);
                    Console.WriteLine("You made the new HIGH SCORE: {0}", score);
                }
                else
                {
                    Console.SetCursorPosition((Console.WindowWidth - "High Score: {0}".Length) / 2, Console.WindowHeight / 2 + 1);
                    Console.WriteLine("High Score: {0}", readText);
                    Console.SetCursorPosition((Console.WindowWidth - "Your Score: {0}".Length) / 2, Console.WindowHeight / 2 + 2);
                    Console.WriteLine("Your Score: {0}", score);
                }
            }
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
