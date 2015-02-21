using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dread_Knight
{
    class SinglePlayer
    {
        struct Object
        {
            public int x;
            public int y;
            public string str;
            public ConsoleColor color;
        }

        static Object ourPlayer = new Object();
        static Random randomGenerator = new Random();
        static List<Object> enemies = new List<Object>();
        static List<Object> shots = new List<Object>();

        static int score = 0;
        static int sizeOfDrawField = 4;
        //static int livesCount = 5;

        internal static void SinglePlay()
        {
            Console.OutputEncoding = Encoding.UTF8;

            //make our player
            ourPlayer.x = 0;
            ourPlayer.y = Console.WindowHeight / 2;
            ourPlayer.str = " ('0.0)-=╦╤── ";
            ourPlayer.color = ConsoleColor.White;

            int step = 0;
            int enemiesPause = 5;

            while (true)
            {
                //check if an enemy is hitted by shot
                CollisionShotAndEnemy();

                //add new enemy every 3 steps
                if (step % enemiesPause == 0)
                {
                    AddNewEnemy();
                    step = 0;
                }

                step++;

                //move our player and shoot(key pressed)
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                    MoveOurPlayer(pressedKey);
                }

                MoveShots();

                MoveEnemies();

                //check if an enemy is hitted by shot
                CollisionShotAndEnemy();

                //clear the console - old positions
                Console.Clear();

                //draw new positions
                RedrawPlayfield();

                //draw info
                PrintInfoOnPosition();

                //slow down program
                Thread.Sleep(100);
            }
        }

        static void CollisionShotAndEnemy()
        {
            List<object> enemiesToRemove = new List<object>();
            List<object> shotsToRemove = new List<object>();

            for (int i = 0; i < enemies.Count; i++)
            {
                for (int j = 0; j < shots.Count; j++)
                {
                    if (enemies[i].x == shots[j].x && enemies[i].y == shots[j].y)
                    {
                        enemiesToRemove.Add(enemies[i]);
                        shotsToRemove.Add(shots[j]);
                        score += 10;
                    }
                }
            }

            List<Object> newListOfEnemies = new List<Object>();
            List<Object> newListOfShots = new List<Object>();

            for (int i = 0; i < enemies.Count; i++)
            {
                if (!enemiesToRemove.Contains(enemies[i]))
                {
                    newListOfEnemies.Add(enemies[i]);
                }
            }

            enemies = newListOfEnemies;

            for (int i = 0; i < shots.Count; i++)
            {
                if (!shotsToRemove.Contains(shots[i]))
                {
                    newListOfShots.Add(shots[i]);
                }
            }

            shots = newListOfShots;
        }

        static void AddNewEnemy()
        {
            Object newEnemy = new Object();
            newEnemy.x = Console.WindowWidth - 1;
            newEnemy.y = randomGenerator.Next(sizeOfDrawField, Console.WindowHeight);
            newEnemy.str = "%";
            newEnemy.color = ConsoleColor.Yellow;
            enemies.Add(newEnemy);
        }

        static void Shoot()
        {
            Object newShot = new Object();
            newShot.x = ourPlayer.x + 1 + 14;   //14 = size of our player
            newShot.y = ourPlayer.y;
            newShot.str = "Ѿ";
            newShot.color = ConsoleColor.Black;
            shots.Add(newShot);
        }

        static void MoveOurPlayer(ConsoleKeyInfo pressedKey)
        {
            if (pressedKey.Key == ConsoleKey.UpArrow)
            {
                if (ourPlayer.y - 1 >= sizeOfDrawField)
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
            else if (pressedKey.Key == ConsoleKey.Spacebar)
            {
                Shoot();
            }
        }

        static void MoveShots()
        {
            List<Object> newListOfShots = new List<Object>();
            for (int i = 0; i < shots.Count; i++)
            {
                Object oldShot = shots[i];
                Object newShot = new Object();
                newShot.x = oldShot.x + 1;
                newShot.y = oldShot.y;
                newShot.str = oldShot.str;
                newShot.color = oldShot.color;

                if (newShot.x < Console.WindowWidth)
                {
                    newListOfShots.Add(newShot);
                }
            }

            shots = newListOfShots;
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
                    shots.Clear();

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
            foreach (Object shot in shots)
            {
                PrintOnPosition(shot.x, shot.y, shot.str, shot.color);
            }
        }

        static void PrintOnPosition(int x, int y, string str, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(str);
        }

        static void PrintInfoOnPosition()
        {
            string line = new string('-', Console.WindowWidth);
            PrintOnPosition(0, 1, "Score " + score, ConsoleColor.Black);
            PrintOnPosition(0, 2, "Lives ", ConsoleColor.Black);
            PrintOnPosition(Console.WindowWidth / 2, 0, "Level ", ConsoleColor.Black);
            PrintOnPosition(Console.WindowWidth / 2, 2, "Time", ConsoleColor.Black);
            PrintOnPosition(0, 3, "" + line, ConsoleColor.Black);
        }
    }
}
