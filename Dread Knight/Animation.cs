using System;
using System.Threading;

namespace Dread_Knight
{
    class Animation
    {
        struct Object
        {
            public int x;
            public int y;
            public string s;
            public ConsoleColor color;
        }

        static void PrintOnPosition(int x, int y, string s, ConsoleColor color = ConsoleColor.Black)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(s);
        }

        internal static void Castle()
        {
            Console.WriteLine(@"
                                                                                                                                                                                                                                                                                                                  
                                                                                                      
                                 _  ___ _ _ _                     _   _ _        _                                      
                                | |/ (_) | (_)_ __   __ _        | \ | (_)_ __  (_) __ _ ___                            
                                | ' /| | | | | '_ \ / _` |       |  \| | | '_ \ | |/ _` / __|                           
                                | . \| | | | | | | | (_| |       | |\  | | | | || | (_| \__ \                           
                                |_|\_\_|_|_|_|_| |_|\__, |       |_| \_|_|_| |_|/ |\__,_|___/                           
                                                    |___/                     |__/                                                                                                                          
            !_                             !_                                         
            |* `--,                        |* `--,                               ^                     
            |.-`                           |.-`                                 / \                    
            |                              |                                   /   \                   
            ^                              ^                                  /     \                   
           / \                            / \                                /       \                
          /   \                          /   \                             _/  _   _  \_              
         /     \                        /     \                           [ ]_[ ]_[ ]_[ ]       
        /       \                      /       \[ ]_[ ]_[ ]_[ ]_[ ]       |_           _|                           
      _/  _   _  \_                  _/  _   _  \|_ _   _ _   _ _|         |           |                            
     [ ]_[ ]_[ ]_[ ]                [ ]_[ ]_[ ]_[ ]             |          |      []   |\              
     |_           _|                |_           _|        []   |          |           | \             
      |           |                  |           |              |          |           |  \            
      |           |_[ ]_[ ]__[ ]_[ ]_|           |_[ ]_[ ]_[ ]_[ ]_[ ]_[ ]_|      []   |___\           
      |           |                  |           |                         |           |/+\|           
      |           |                  |           |                         |           ||+||            
      |     _     |                  |     _     |      []         []      |           ||+||            
      |    /+\    |                  |    /+\    |                         |           |^^^|          
      |   |+|+|   |                  |   |+|+|   |                         |           |   |            
      |   |+|+|   |     _,-^-,_      |   |+|+|   |                         |           |   |            
      |   |+|+|   |    / |   | \     |   |+|+|   |                         |           |  /            
      |   ^^^^^   |    | |   | |     |   ^^^^^   |                         |           | /              
      |           |    | |   <&>     |           |                         |           |/               
      |           |    | |   | |     |           |                         |           |                
      |           |    | |   | |     |           |                         |           |                
`^^^^^^^^^^^`^^^^`^^^`^^^^^^^^^^^^`^`^`^^^^^`^^^^^^^^^`^^^^^^^^^`^^^^^^^^^`^^^^`^^^`^^^^^^^`^^^^`^^^`^^^^^^^^`^^^^`^^^`^^^^
");
                                                /* New logo - even uglier :) */
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(25, 21);
            Console.Write("▄   ▄");
            Console.SetCursorPosition(24, 22);
            Console.Write("▀ ▀▄▀ ▀");
            Console.SetCursorPosition(25, 23);
            Console.Write("▄▀ ▀▄");
            Console.SetCursorPosition(26, 24);
            Console.Write("▀▄▀");
        }

        internal static void FirstStage()
        {
            Object dreadNight = new Object();
            dreadNight.x = 100;
            dreadNight.y = 31;
            dreadNight.s = "(0.0')";
            dreadNight.color = ConsoleColor.White;

            while (dreadNight.x > 24)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Castle();
                PrintOnPosition(dreadNight.x, dreadNight.y, dreadNight.s, dreadNight.color);
                dreadNight.x = dreadNight.x - 8;
                Thread.Sleep(500);
            }
        }
    }
}
