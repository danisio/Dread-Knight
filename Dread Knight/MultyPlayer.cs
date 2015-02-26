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

        static Object firstPlayer = new Object();
        static Object secondPlayer = new Object();
        static Random randomGenerator = new Random();
        static List<Object> enemies = new List<Object>();
        static List<Object> shots = new List<Object>();
        static List<Object> rocks = new List<Object>();

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

        static int stepEnemy = 0;
        static int enemiesPause = 13;
        static int stepRocks = 0;
        static int rocksPause = 30;

        //static int livesCount = 5;
        static int playerOneLives = 5;
        static int playerTwoLives = 5;

        internal static void MultyPlay(bool isMulti = false)
        {
            Console.OutputEncoding = Encoding.Unicode;

            //make first player
            firstPlayer.x = 0;
            firstPlayer.y = Console.WindowHeight / 2;
            firstPlayer.str = " ('0.0)-=╦╤── ";
            firstPlayer.color = ConsoleColor.Yellow;

            //make second Player
            if (isMulti)
            {
                secondPlayer.x = 0;
                secondPlayer.y = Console.WindowHeight / 2 + 1;
                secondPlayer.str = " ('■_■)-=╦╤── ";
                secondPlayer.color = ConsoleColor.Yellow;
            }

            while (true)
            {
                //check if an enemy is hitted by shot
                CollisionShotAndEnemy();

                //add new enemy every "enemiesPause" step
                if (stepEnemy % enemiesPause == 0)
                {
                    AddNewEnemy();
                    stepEnemy = 0;
                }

                stepEnemy++;

                //add new rock every "rocksPause" step
                if (stepRocks % rocksPause == 0)
                {
                    AddNewRock();
                    stepRocks = 0;
                }

                stepRocks++;

                //move our player and shoot(key pressed)
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                    MoveFirstPlayer(pressedKey);
                    MoveSecondPlayer(pressedKey);
                }

                MoveShots();

                MoveEnemies(isMulti);

                MoveRocks();

                Console.Clear();

                //draw new positions
                RedrawPlayfield();

                //draw info
                PrintInfoOnPosition(isMulti);

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
                ConsoleColor.Green, 
                ConsoleColor.DarkGreen, 
                ConsoleColor.Red,
                ConsoleColor.Magenta,
                ConsoleColor.White
            };

            newEnemy.color = enemyColors[randomGenerator.Next(0, enemyColors.Length)];
            enemies.Add(newEnemy);
        }

        static void AddNewRock()
        {
            Object newRock = new Object();
            newRock.x = Console.WindowWidth - 1;
            newRock.y = randomGenerator.Next(sizeOfDrawField, Console.WindowHeight);
            newRock.str = "▓"; //█
            newRock.color = ConsoleColor.Red;
            rocks.Add(newRock);
        }

        static void Shoot(Object player)
        {
            Object newShot = new Object();
            newShot.x = player.x + player.str.Length;
            newShot.y = player.y;
            newShot.str = "●";
            newShot.color = ConsoleColor.Yellow;
            shots.Add(newShot);
        }

        static void MoveFirstPlayer(ConsoleKeyInfo pressedKey)
        {
            if (pressedKey.Key == ConsoleKey.UpArrow)
            {
                if (firstPlayer.y - 1 >= sizeOfDrawField)
                {
                    firstPlayer.y = firstPlayer.y - 1;
                }
            }
            else if (pressedKey.Key == ConsoleKey.DownArrow)
            {
                if (firstPlayer.y + 1 < Console.WindowHeight)
                {
                    firstPlayer.y = firstPlayer.y + 1;

                }
            }
            else if (pressedKey.Key == ConsoleKey.Spacebar)
            {
                Shoot(firstPlayer);
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
                Shoot(secondPlayer);
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

        static void MoveEnemies(bool isMulti)
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

                if (isMulti)
                {
                    for (int j = 0; j < firstPlayer.str.Length; j++)                                //
                    {                                                                             //
                        if ((newEnemy.x == firstPlayer.x + j && newEnemy.y == firstPlayer.y) ||
                            (newEnemy.x == secondPlayer.x + j && newEnemy.y == secondPlayer.y))
                        {                                                                         //
                            //livesCount--;                                                       //
                            Console.Beep(1000, 50);                                               // Checks every part of both players for collision with the enemy
                            enemies.Clear();                                                      //
                            shots.Clear();
                            rocks.Clear();
                            collisionPlayerEnemy = true;                                          //
                            break;                                                                //
                            // console.writeline environment.exit(0)                              //
                        }                                                                         //
                    }
                }
                else
                {
                    for (int j = 0; j < firstPlayer.str.Length; j++)                                //
                    {                                                                             //
                        if (newEnemy.x == firstPlayer.x + j && newEnemy.y == firstPlayer.y)
                        {                                                                         //
                            //livesCount--;                                                       //
                            Console.Beep(1000, 50);                                               // Checks every part of our player for collision with the enemy
                            enemies.Clear();                                                      //
                            shots.Clear();
                            rocks.Clear();
                            collisionPlayerEnemy = true;                                          //
                            break;                                                                //
                            // console.writeline environment.exit(0)                              //
                        }                                                                         //
                    }
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

        static void MoveRocks()
        {
            List<Object> newListOfRocks = new List<Object>();
            for (int i = 0; i < rocks.Count; i++)
            {
                Object oldRock = rocks[i];
                Object newRock = new Object();
                newRock.x = oldRock.x - 1;
                newRock.y = oldRock.y;
                newRock.str = oldRock.str;
                newRock.color = oldRock.color;

                if (newRock.x <= firstPlayer.x + firstPlayer.str.Length && newRock.y == firstPlayer.y)
                {
                    score -= 50;
                    if (score < 0)
                    {
                        score = 0;
                    }
                }
                else
                {
                    if (newRock.x > 0)
                    {
                        newListOfRocks.Add(newRock);
                    }
                }
            }

            rocks = newListOfRocks;
        }

        static void RedrawPlayfield()
        {
            PrintOnPosition(firstPlayer.x, firstPlayer.y, firstPlayer.str, firstPlayer.color);
            PrintOnPosition(secondPlayer.x, secondPlayer.y, secondPlayer.str, secondPlayer.color);

            foreach (Object enemy in enemies)
            {
                PrintOnPosition(enemy.x, enemy.y, enemy.str, enemy.color, true);
            }
            foreach (Object shot in shots)
            {
                PrintOnPosition(shot.x, shot.y, shot.str, shot.color);
            }
            foreach (Object rock in rocks)
            {
                PrintOnPosition(rock.x, rock.y, rock.str, rock.color);
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

        static void PrintInfoOnPosition(bool isMulti)
        {
            string line = new string('-', Console.WindowWidth);
            string livesOne = new string('♥', playerOneLives);
            string livesTwo = new string('♥', playerTwoLives);
            PrintOnPosition(1, 0, "Player 1", ConsoleColor.White);
            PrintOnPosition(1, 2, "Lives " , ConsoleColor.White);
            PrintOnPosition(7, 2, livesOne, ConsoleColor.Red);
            if (isMulti)
            {
                PrintOnPosition(Console.WindowWidth - 12, 0, "Player 2", ConsoleColor.White);
                PrintOnPosition(Console.WindowWidth - 12, 2, "Lives " , ConsoleColor.White);
                PrintOnPosition(Console.WindowWidth - 6, 2, livesTwo, ConsoleColor.Blue);
            }
            PrintOnPosition(Console.WindowWidth / 2, 0, "Level " + currentLevel, ConsoleColor.White);
            PrintOnPosition(Console.WindowWidth / 2, 1, "Score " + score, ConsoleColor.White);
            PrintOnPosition(Console.WindowWidth / 2, 2, "Time", ConsoleColor.White);
            PrintOnPosition(0, 3, "" + line, ConsoleColor.White);
        }
    }
}
