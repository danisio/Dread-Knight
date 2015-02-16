using System;
using System.Threading;


namespace Dread_Knight
{
    class Intro
    {
        internal static int Menu()
        {
            Console.Clear();
            Console.WriteLine("── Hello, Doncho!");
            Thread.Sleep(950);
            Console.WriteLine("── Hello, Dread Knight!");
            Thread.Sleep(950);
            Console.WriteLine("── Do you want to play with me?");
            Console.WriteLine("   press: Y/N");
            ConsoleKeyInfo pressedKey = Console.ReadKey();
            while (pressedKey.Key != ConsoleKey.Y && pressedKey.Key != ConsoleKey.N)
            {
                Console.Clear();
                Console.WriteLine("── Hello, Doncho!\n── Hello, Dread Knight!\n── Do you want to play with me?");
                Console.WriteLine("press: Y/N");
                pressedKey = Console.ReadKey();
            }
            Console.Clear();
            if (pressedKey.Key == ConsoleKey.Y)
                return 2;
            if (pressedKey.Key == ConsoleKey.N)
                return 1;
            return 1;
        }
    }
}
