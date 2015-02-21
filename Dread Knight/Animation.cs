using System;
using System.Threading;

namespace Dread_Knight
{
    class Animation
    {
        static void PrintOnPosition(int x, int y, string s, ConsoleColor color = ConsoleColor.Black)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(s);
        }

        internal static void Castle()
        {
            Console.WriteLine("                                                                                                         ");
            Console.WriteLine("                                                                                                         ");
            Console.WriteLine("                                                                                                         ");
            Console.WriteLine("                                                                                                         ");
            Console.WriteLine("                                                                                 ^                       ");
            Console.WriteLine("                                                                                / \\                      ");
            Console.WriteLine("            !_                             !_                                  /   \\                     ");
            Console.WriteLine("            |* `--,                        |* `--,                            /     \\                    ");
            Console.WriteLine("            |.-`                           |.-`                              /       \\                   ");
            Console.WriteLine("            |                              |                               _/  _   _  \\_                 ");
            Console.WriteLine("            ^                              ^                              [ ]_[ ]_[ ]_[ ]                ");
            Console.WriteLine("           / \\                            / \\                             |_           _|                ");
            Console.WriteLine("          /   \\                          /   \\                             |           |                 ");
            Console.WriteLine("         /     \\                        /     \\                            |      []   |                 ");
            Console.WriteLine("        /       \\                      /       \\[ ]_[ ]_[ ]_[ ]_[ ]        |           |                 ");
            Console.WriteLine("      _/  _   _  \\_                  _/  _   _  \\|_ _   _ _   _ _|         |           |                 ");
            Console.WriteLine("     [ ]_[ ]_[ ]_[ ]                [ ]_[ ]_[ ]_[ ]             |          |      []   |\\                ");
            Console.WriteLine("     |_           _|                |_           _|        []   |          |           | \\               ");
            Console.WriteLine("      |           |                  |           |              |          |           |  \\              ");
            Console.WriteLine("      |           |_[ ]_[ ]__[ ]_[ ]_|           |_[ ]_[ ]_[ ]_[ ]_[ ]_[ ]_|      []   |___\\             ");
            Console.WriteLine("      |           |                  |           |                         |           |/+\\|             ");
            Console.WriteLine("      |           |                  |           |                         |           ||+||             ");
            Console.WriteLine("      |     _     |                  |     _     |      []         []      |           ||+||             ");
            Console.WriteLine("      |    /+\\    |                  |    /+\\    |                         |           |^^^|             ");
            Console.WriteLine("      |   |+|+|   |                  |   |+|+|   |                         |           |   |             ");
            Console.WriteLine("      |   |+|+|   |      _,--,_      |   |+|+|   |                         |           |   |             ");
            Console.WriteLine("      |   |+|+|   |     / |  | \\     |   |+|+|   |                         |           |  /              ");
            Console.WriteLine("      |   ^^^^^   |     | |  | |     |   ^^^^^   |                         |           | /               ");
            Console.WriteLine("      |           |     | |  <&>     |           |                         |           |/                ");
            Console.WriteLine("      |           |     | |  | |     |           |                         |           |                 ");
            Console.WriteLine("      |           |     | |  | |     |           |                         |           |                 ");
            Console.WriteLine("`^^^^^^^^^^^`^^^^`^^^`^^^^^^^^^^^^`^`^`^^^^^`^^^^^^^^^`^^^^^^^^^`^^^^^^^^^`^^^^`^^^`^^^^^^^`^^^^`^^^`^^^^^^^^`^^^^`^^^`^^^^");

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(26, 21);
            Console.Write("/\\/\\");
            Console.SetCursorPosition(27, 22);
            Console.Write("/\\");
            Console.SetCursorPosition(27, 23);
            Console.Write("\\/");
            Console.ForegroundColor = ConsoleColor.Black;
        }

        internal static void FirstStage()
        {
            Object dreadNight = new Object();
            dreadNight.x = 100;
            dreadNight.y = 30;
            dreadNight.s = "(0.0')";
            dreadNight.color = ConsoleColor.Black;

            while (dreadNight.x > 24)
            {
                Console.Clear();
                Castle();
                PrintOnPosition(dreadNight.x, dreadNight.y, dreadNight.s, dreadNight.color);
                dreadNight.x = dreadNight.x - 1;
                Thread.Sleep(10);
            }
        }
    }
}
