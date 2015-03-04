using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace Dread_Knight
{
    class End
    {
        internal static void GameOver(int score)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(0, 0);
            string gameOver = @"


                                 _____          __  __ ______    ______      ________ _____  
                                / ____|   /\   |  \/  |  ____|  / __ \ \    / /  ____|  __ \ 
                               | |  __   /  \  | \  / | |__    | |  | \ \  / /| |__  | |__) |
                               | | |_ | / /\ \ | |\/| |  __|   | |  | |\ \/ / |  __| |  _  / 
                               | |__| |/ ____ \| |  | | |____  | |__| | \  /  | |____| | \ \ 
                                \_____/_/    \_\_|  |_|______|  \____/   \/   |______|_|  \_\
                                                              
";
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(gameOver);

            for (int i = 1; i < 7; i++)
            {
                Console.Beep(498 - i * 64, 300);
            }

            Task.Run(() =>
            {
                SpeechSynthesizer synth = new SpeechSynthesizer();
                synth.Rate = -5;
                synth.Volume = 100;
                synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Senior);
                synth.SpeakAsync("Game over lamer!");
            });

            Console.Beep(70, 1000);
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);

            string path = "score.txt";

            try
            {
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
            }
            catch (FileLoadException)
            {
                Console.WriteLine("Unable to load file with High Scores");
            }

            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
