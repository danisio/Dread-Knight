using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dread_Knight
{
  
    class SinglePlayer
    {  struct Object
    {
        public int x;
        public int y;
        public string str;
        public ConsoleColor color;
    }

        static Object ourPlayer = new Object();
        static Random randomGenerator = new Random();
        static List<Object> enemies = new List<Object>();
        //static int livesCount = 5;

        internal static void SinglePlay()
        {
            ourPlayer.x = 1;

            //make our playern
            ourPlayer.x = 0;
            ourPlayer.y = Console.WindowHeight / 2;
            ourPlayer.str = " ('0.0)-=╦╤── ";
            ourPlayer.color = ConsoleColor.White;

            int step = 0;
            int enemiesPause = 5;
            while (true)
            {
                //add new enemy every 3 steps
                if (step % enemiesPause == 0)
                {
                    AddNewEnemy();
                    step = 0;
                }
                step++;

                //move our player(key pressed)
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                    MoveOurPlayer(pressedKey);
                }

                MoveEnemies();

                //clear the console
                Console.Clear();

                RedrawPlayfield();

                //draw info
                //PrintInfoOnPosition();

                //slow down program
                Thread.Sleep(100);

            }
        }
        static void AddNewEnemy()
        {
            Object newEnemy = new Object();
            newEnemy.x = Console.WindowWidth - 1;
            newEnemy.y = randomGenerator.Next(0, Console.WindowHeight);
            newEnemy.str = "%";
            newEnemy.color = ConsoleColor.Yellow;
            enemies.Add(newEnemy);

        }

        static void MoveOurPlayer(ConsoleKeyInfo pressedKey)
        {
            if (pressedKey.Key == ConsoleKey.UpArrow)
            {
                if (ourPlayer.y - 1 >= 0)
                {
                    ourPlayer.y = ourPlayer.y - 1;
                }
            }
            else if (pressedKey.Key == ConsoleKey.DownArrow)
            {
                if (ourPlayer.y + 1 < Console.WindowHeight)
                {
                    ourPlayer.y = ourPlayer.y + 1;

                }
            }
        }

        static void MoveEnemies()
        {
            List<Object> newListOfEnemies = new List<Object>();
            for (int i = 0; i < enemies.Count; i++)
            {
                Object oldEnemy = enemies[i];
                Object newEnemy = new Object();
                newEnemy.x = oldEnemy.x - 1;
                newEnemy.y = oldEnemy.y;
                newEnemy.str = oldEnemy.str;
                newEnemy.color = oldEnemy.color;
                if (newEnemy.x == ourPlayer.x && newEnemy.y == ourPlayer.y)
                {
                    //livesCount--;
                    Console.Beep(1000, 50);
                    enemies.Clear();
                    // console.writeline environment.exit(0)
                }

                if (newEnemy.x > 0)
                {
                    newListOfEnemies.Add(newEnemy);
                }
            }

            enemies = newListOfEnemies;
        }

        static void RedrawPlayfield()
        {
            PrintOnPosition(ourPlayer.x, ourPlayer.y, ourPlayer.str, ourPlayer.color);
            foreach (Object enemy in enemies)
            {
                PrintOnPosition(enemy.x, enemy.y, enemy.str, enemy.color);
            }

        }

        static void PrintOnPosition(int x, int y, string str, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(str);
        }
    }

}
