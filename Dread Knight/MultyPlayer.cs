using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dread_Knight
{
    class MultyPlayer
    {
        struct Object
        {
            public int x;
            public int y;
            public string str;
            public ConsoleColor color;
        }

        static Object ourPlayer = new Object();
        static Object secondPlayer = new Object();
        static Random randomGenerator = new Random();
        static List<Object> enemies = new List<Object>();
        static List<Object> shots = new List<Object>();

        static int[,] levelsData = //array with data about levels -> level 1-100 points, level 2-200 points etc.
                    {{1, 100},
                     {2, 200},
                     {3, 300},
                     {4, 400},
                     {5, 500}};

        static int currentLevel = levelsData[0, 0]; //set level 1 when the game starts
        static int maxPointsForCurrentLevel = levelsData[currentLevel - 1, 1]; //set max points for current level -->100
        static int maxLevel = levelsData[4, 0]; //set max level --> 5

        static int score = 0;
        static int sizeOfDrawField = 4;
        static int speed = 0; //used for acceleration of the game 
        static int acceleration = 30;

        static int step = 0;
        static int enemiesPause = 5;

        //static int livesCount = 5;

        internal static void MultyPlay()
        {
            Console.OutputEncoding = Encoding.UTF8;

            //make our player
            ourPlayer.x = 0;
            ourPlayer.y = Console.WindowHeight / 2;
            ourPlayer.str = " ('0.0)-=╦╤── ";
            ourPlayer.color = ConsoleColor.White;

            //make second Player
            secondPlayer.x = 0;
            secondPlayer.y = Console.WindowHeight / 2 + 1;
            secondPlayer.str = " ('■_■)-=╦╤── ";
            secondPlayer.color = ConsoleColor.White;


            while (true)
            {
                //check if an enemy is hitted by shot
                CollisionShotAndEnemy();

                //add new enemy every 5 steps
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
                    MoveSecondPlayer(pressedKey);
                }

                MoveShots();

                MoveEnemies();

                //clear the console - old positions
                Console.Clear();

                //draw new positions
                RedrawPlayfield();

                //draw info
                PrintInfoOnPosition();

                //slow down program
                Thread.Sleep(150 - speed);
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
                    if ((enemies[i].x == shots[j].x && enemies[i].y == shots[j].y) ||  //check current positions of enemy and shot 
                         (enemies[i].x == shots[j].x + 1 && enemies[i].y == shots[j].y))  //same enemy and next position of the shot (avoids mismatch shot and enemy)
                    {
                        enemiesToRemove.Add(enemies[i]);
                        shotsToRemove.Add(shots[j]);
                        score += 10;

                        if (score >= maxPointsForCurrentLevel)
                        {
                            if (currentLevel + 1 <= maxLevel)       // verify whether the last level is reached
                            {
                                currentLevel += 1;
                                maxPointsForCurrentLevel = levelsData[currentLevel - 1, 1];
                                speed += acceleration;              //every next level will be faster
                                enemiesPause--;                     //every next level will be added more enemies 

                                if (currentLevel == maxLevel)
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                }
                            }
                        }
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
            //  How enemies would look
            string[] enemyLooks = new string[] { ".\\/.", ".\\,,/.", "o\\_/o", "*\\)_(/*", "+|,,,|+", "'\\]..[/'", "(niki=<", "<<-ivo[#", "-=evlogi{" };

            Object newEnemy = new Object();
            newEnemy.x = Console.WindowWidth - 1;
            newEnemy.y = randomGenerator.Next(sizeOfDrawField, Console.WindowHeight);
            newEnemy.str = enemyLooks[randomGenerator.Next(0, enemyLooks.Length)];
            ConsoleColor[] enemyColors = 
            {
                ConsoleColor.Yellow, 
                ConsoleColor.DarkGreen, 
                ConsoleColor.Red,
                ConsoleColor.DarkMagenta,
                ConsoleColor.Blue
            };
            newEnemy.color = enemyColors[randomGenerator.Next(0, enemyColors.Length)];
            enemies.Add(newEnemy);
        }

        static void ShootFirstPlayer()
        {
            Object newShotFP = new Object();
            newShotFP.x = ourPlayer.x + 14;   //14 = size of our player
            newShotFP.y = ourPlayer.y;
            newShotFP.str = "Ѿ";
            newShotFP.color = ConsoleColor.Black;
            shots.Add(newShotFP);
        }

        static void ShootSecondPlayer()
        {
            Object newShotSP = new Object();
            newShotSP.x = secondPlayer.x + 14;   //14 = size of our player
            newShotSP.y = secondPlayer.y;
            newShotSP.str = "Ѿ";
            newShotSP.color = ConsoleColor.Black;
            shots.Add(newShotSP);
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
                ShootFirstPlayer();
            }
        }

        static void MoveSecondPlayer(ConsoleKeyInfo pressedKey)
        {
            if (pressedKey.Key == ConsoleKey.W)
            {
                if (secondPlayer.y - 1 >= sizeOfDrawField)
                {
                    secondPlayer.y = secondPlayer.y - 1;
                }
            }
            else if (pressedKey.Key == ConsoleKey.S)
            {
                if (secondPlayer.y + 1 < Console.WindowHeight)
                {
                    secondPlayer.y = secondPlayer.y + 1;

                }
            }
            else if (pressedKey.Key == ConsoleKey.Tab)
            {
                ShootSecondPlayer();
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

                Object oldSPShot = shots[i];
                Object newSPShot = new Object();
                newSPShot.x = oldSPShot.x + 1;
                newSPShot.y = oldSPShot.y;
                newSPShot.str = oldSPShot.str;
                newSPShot.color = oldSPShot.color;

                if (newShot.x < Console.WindowWidth & newSPShot.x < Console.WindowWidth)
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

                bool collisionPlayerEnemy = false;

                for (int j = 0; j < ourPlayer.str.Length; j++)                                //
                {                                                                             //
                    if ((newEnemy.x == ourPlayer.x + j && newEnemy.y == ourPlayer.y) || 
                        (newEnemy.x == secondPlayer.x + j && newEnemy.y == secondPlayer.y))
                    {                                                                         //
                        //livesCount--;                                                       //
                        Console.Beep(1000, 50);                                               // Checks every part of both players for collision with the enemy
                        enemies.Clear();                                                      //
                        shots.Clear();                                                        //
                        collisionPlayerEnemy = true;                                          //
                        break;                                                                //
                        // console.writeline environment.exit(0)                              //
                    }                                                                         //
                }

                if (collisionPlayerEnemy)
                {
                    break;
                }


                if (newEnemy.x > 0)
                {
                    newListOfEnemies.Add(newEnemy);
                }
                else if (newEnemy.x == 0)                               //
                {                                                       //
                    newEnemy.x++;                                       //
                                                                        //  
                    string tempNewEnemy = string.Empty;                 //  
                    for (int k = 1; k < newEnemy.str.Length; k++)       //  
                    {                                                   //
                        tempNewEnemy += newEnemy.str[k];                //
                    }                                                   //  Checks if the enemy reached the end of the field.
                                                                        //  If yes, its string is gradually trimmed from its beginning.
                    if (newEnemy.str.Length == 0)                       //
                    {                                                   //
                        continue;                                       //
                    }                                                   //
                                                                        //
                    newEnemy.str = tempNewEnemy;                        //
                    newListOfEnemies.Add(newEnemy);                     //
                }                              
            }

            enemies = newListOfEnemies;
        }

        static void RedrawPlayfield()
        {
            PrintOnPosition(ourPlayer.x, ourPlayer.y, ourPlayer.str, ourPlayer.color);
            PrintOnPosition(secondPlayer.x, secondPlayer.y, secondPlayer.str, secondPlayer.color);

            foreach (Object enemy in enemies)
            {
                PrintOnPosition(enemy.x, enemy.y, enemy.str, enemy.color, true);
            }
            foreach (Object shot in shots)
            {
                PrintOnPosition(shot.x, shot.y, shot.str, shot.color);
            }
        }

        static void PrintOnPosition(int x, int y, string str, ConsoleColor color = ConsoleColor.Gray, bool isEnemy = false)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;

            if (isEnemy)                                                  //    
            {                                                             //    
                string tempString = str;                                  //    
                int tempLenght = (Console.WindowWidth - 1) - x;           //    
                if (tempLenght < tempString.Length)                       //    
                {                                                         //    Fix for the bug with newly generated enemy's string-tail showing on the left side of the field.
                    str = string.Empty;                                   //    
                    for (int i = 0; i < tempLenght; i++)                  //    
                    {                                                     //    
                        str += tempString[i];                             //    
                    }                                                     //    
                }                                                         //    
            }

            Console.Write(str);
        }

        static void PrintInfoOnPosition()
        {
            string line = new string('-', Console.WindowWidth);
            PrintOnPosition(0, 0, "Player 1", ConsoleColor.Black);
            PrintOnPosition(0, 1, "Score " + score, ConsoleColor.Black);
            PrintOnPosition(0, 2, "Lives ", ConsoleColor.Black);
            PrintOnPosition(Console.WindowWidth / 2, 0, "Level " + currentLevel, ConsoleColor.Black);
            PrintOnPosition(Console.WindowWidth / 2, 2, "Time", ConsoleColor.Black);
            PrintOnPosition(0, 3, "" + line, ConsoleColor.Black);
        }
    }
}
