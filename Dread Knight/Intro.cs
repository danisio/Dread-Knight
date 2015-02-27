using System;
using System.Threading;


namespace Dread_Knight
{
    class Intro
    {
        static void PrintOnPosition(int x, int y, string s, ConsoleColor color = ConsoleColor.Black)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(s);
        }

        internal static int Menu()
        {
            Console.Clear();
            Console.SetCursorPosition(20, 20);
            Console.WriteLine("('0.0)");
            Console.SetCursorPosition(60, 20);
            Console.WriteLine("(■_■')");
            //Thread.Sleep(950);
            Console.SetCursorPosition(20, 18);
            Console.WriteLine("── Hello, Doncho!");
            //Thread.Sleep(950);
            Console.SetCursorPosition(60, 18);
            Console.WriteLine("── Hello, Dread Knight!");
            //Thread.Sleep(950);
            Console.SetCursorPosition(20, 19);
            Console.WriteLine("── Do you want some action?");
            //Thread.Sleep(950);
            Console.SetCursorPosition(53, 10);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("   PRESS Y or N");
            ConsoleKeyInfo pressedKey = Console.ReadKey(true);
            while (pressedKey.Key != ConsoleKey.Y && pressedKey.Key != ConsoleKey.N)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(20, 20);
                Console.WriteLine("('0.0)");
                Console.SetCursorPosition(60, 20);
                Console.WriteLine("(■_■')");
                Console.SetCursorPosition(20, 18);
                Console.WriteLine("── Hello, Doncho!");
                Console.SetCursorPosition(60, 18);
                Console.WriteLine("── Hello, Dread Knight!");
                Console.SetCursorPosition(20, 19);
                Console.WriteLine("── Do you want some action?");
                Console.SetCursorPosition(53, 10);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("   PRESS Y or N");
                pressedKey = Console.ReadKey();
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (pressedKey.Key == ConsoleKey.Y)
            {
                Console.SetCursorPosition(60, 19);
                Console.WriteLine("── Sir, YES, Sir!");
                Thread.Sleep(950);
                Console.Clear();
                return 2;
            }
            if (pressedKey.Key == ConsoleKey.N)
            {
                Console.SetCursorPosition(60, 19);
                Console.WriteLine("── I've got some bad news, Sir.");
                Thread.Sleep(1250);
                Console.Clear();
                return 1;
            }
            return 1;
        }
    }
}
