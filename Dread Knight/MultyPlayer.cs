using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Media;

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
        static List<Object> bonuses = new List<Object>();

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

        static int playerOneLives = 5;
        static int playerTwoLives = 5;
        static bool playerOneDied;
        static bool playerTwoDied;

        static int playerOneAmmo = 10;
        static int playerTwoAmmo = 10;

        static int time = 0;
        
        internal static void MultyPlay(bool isMulti = false)
        {
            Console.OutputEncoding = Encoding.Unicode;

            DrawPlayers(isMulti);

            bool musicOnOff = true;

            SoundPlay(false);

            while (true)
            {
                //check if an enemy is hitted by shot
                CollisionShotAndEnemy();
                CollisionShotAndRock();


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

                //add bonus "live"
                int chance = randomGenerator.Next(0, 150);
                if (chance < 1)
                {
                    AddNewBonusObject();
                }

                //move players and shoot(key pressed)
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                    if (pressedKey.KeyChar == 'p')                              //  
                    {                                                           //  
                        Console.Clear();                                        //  
                        Console.SetCursorPosition(55, 15);                      //  
                        Console.WriteLine(           @"╔═╗╔═╗╦ ╦╔═╗╔═╗          
                                                       ╠═╝╠═╣║ ║╚═╗║╣ 
                                                       ╩  ╩ ╩╚═╝╚═╝╚═╝");
                        Console.SetCursorPosition(53, 20);                      //  PAUSE
                        Console.WriteLine("Press P to continue");               //  
                        do                                                      //  
                        {                                                       //  
                            pressedKey = Console.ReadKey(true);                 //  
                        } while (pressedKey.KeyChar != 'p');                    //  
                        RedrawPlayfield();                                      //
                        continue;                                               //  
                    }                                                           //  
                    
                    if (pressedKey.KeyChar == 'm')                              //
                    {                                                           //
                        if (musicOnOff)                                         //
                        {                                                       //
                            SoundPlay(musicOnOff);                              //
                            musicOnOff = false;                                 //  MUSIC ON / OFF
                        }                                                       //
                        else                                                    //
                        {                                                       //
                            SoundPlay(musicOnOff);                              //
                            musicOnOff = true;                                  //
                        }                                                       //
                    }

                    MoveFirstPlayer(pressedKey);
                    if (isMulti)
                    {
                        MoveSecondPlayer(pressedKey);
                    }
                }
                
                time++;
                if (time == 10)
                {
                    score = score + 1;
                    time = 0;
                }
                MoveShots();

                if (MoveEnemies(isMulti) == 0)
                {
                    SoundPlay(true);
                    End.GameOver(score);
                }

                if (MoveRocks(isMulti) == 0)
                {
                    SoundPlay(true);
                    End.GameOver(score);
                }
                
                MoveBonusObject(isMulti);

                // If playerOne dies, it takes player two parameters
                if (playerOneDied)
                {
                    //playerOneDied = false;   UNNECESSARY -> prevents printing second player info alone
                    isMulti = false;
                    firstPlayer.str = secondPlayer.str;
                    firstPlayer.color = secondPlayer.color;
                }

                // If player two dies --> single player
                if (playerTwoDied)
                {
                    isMulti = false;
                }

                Console.Clear();

                //draw new positions
                RedrawPlayfield(isMulti);

                //draw info
                PrintInfoOnPosition(isMulti);

                //slow down program
                Thread.Sleep(150 - speed);
            }
        }

        private static void SoundPlay(bool isStoped = false)
        {
            SoundPlayer sound = new SoundPlayer();
            try
            {
                sound = new SoundPlayer("../../intro.wav");
                sound.PlayLooping();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File intro.wav not found");
                Thread.Sleep(3000);
                return;
            }
            if (isStoped)
            {
                sound.Stop();
            }
            else
            {
                sound.PlayLooping();
            }
        }

        static void DrawPlayers(bool isMulti)
        {
            firstPlayer.x = 0;
            firstPlayer.y = Console.WindowHeight / 2;
            firstPlayer.str = " ('0.0)-=╦╤── ";
            firstPlayer.color = ConsoleColor.White;

            if (isMulti)
            {
                secondPlayer.x = 0;
                secondPlayer.y = Console.WindowHeight / 2 + 1;
                secondPlayer.str = " ('■_■)-=╦╤── ";
                secondPlayer.color = ConsoleColor.Green;
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
                        (enemies[i].x == shots[j].x + 1 && enemies[i].y == shots[j].y) ||  //same enemy and next position of the shot (avoids mismatch shot and enemy)
                        (enemies[i].x == shots[j].x + 2 && enemies[i].y == shots[j].y) ||
                        (enemies[i].x == shots[j].x + 3 && enemies[i].y == shots[j].y))
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
        static void CollisionShotAndRock()
        {
            List<object> shotsToRemove = new List<object>();

            for (int i = 0; i < rocks.Count; i++)
            {
                for (int j = 0; j < shots.Count; j++)
                {
                    if ((rocks[i].x == shots[j].x && rocks[i].y == shots[j].y) ||  //check current positions of enemy and shot 
                        (rocks[i].x == shots[j].x + 1 && rocks[i].y == shots[j].y) ||
                        (rocks[i].x == shots[j].x + 2 && rocks[i].y == shots[j].y) ||
                        (rocks[i].x == shots[j].x + 3 && rocks[i].y == shots[j].y))
                    {
                        shotsToRemove.Add(shots[j]);
                    }
                }
            }

            List<Object> newListOfShots = new List<Object>();
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
            string[] enemyLooks = new string[] { ".\\|/.", ".\\,,/.", "o\\_/o", "*\\)_(/*", "+|,,,|+", "'\\]..[/'", "(niki=<", "<<-ivo[#", "-=evlogi{" };

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
                ConsoleColor.White,
                ConsoleColor.Cyan,
                ConsoleColor.DarkGray
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

        static void AddNewBonusObject()
        {
            string[] bonusLooks = new string[] { "♥", "●" };
            Object newBonus = new Object();
            newBonus.x = Console.WindowWidth - 1;
            newBonus.y = randomGenerator.Next(sizeOfDrawField, Console.WindowHeight);
            newBonus.str = bonusLooks[randomGenerator.Next(0, bonusLooks.Length)];
            newBonus.color = ConsoleColor.Red;
            bonuses.Add(newBonus);
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
                if (playerOneAmmo > 0)
                {
                    playerOneAmmo--;
                    Shoot(firstPlayer);
                }
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
                if (playerTwoAmmo > 0)
                {
                    playerTwoAmmo--;
                    Shoot(secondPlayer);
                }        
            }
        }

        static void MoveShots()
        {
            List<Object> newListOfShots = new List<Object>();
            for (int i = 0; i < shots.Count; i++)
            {
                Object oldShot = shots[i];
                Object newShot = new Object();
                newShot.x = oldShot.x + 3;
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

        static int MoveEnemies(bool isMulti)
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

                if (newEnemy.x == 0 && newEnemy.str.Length > 1)             //  
                {                                                           //  
                    string tempEnemy = string.Empty;                        //  
                    for (int k = 1; k < newEnemy.str.Length; k++)           //  
                    {                                                       //  Enemy gradually disappearing on the left side of the field.
                        tempEnemy += newEnemy.str[k];                       //  
                    }                                                       //  
                    newEnemy.str = tempEnemy;                               //  
                    newEnemy.x++;                                           //  
                }                                                           //  

                if (newEnemy.x <= firstPlayer.x + firstPlayer.str.Length && newEnemy.y == firstPlayer.y)
                {
                    playerOneLives--;
                    score -= 20;
                    if (score < 0)
                    {
                        score = 0;
                    }
                    ClearAllObjects();
                }
                else if (isMulti && (newEnemy.x <= secondPlayer.x + secondPlayer.str.Length && newEnemy.y == secondPlayer.y))
                {
                    playerTwoLives--;
                    score -= 20;
                    if (score < 0)
                    {
                        score = 0;
                    }
                    ClearAllObjects(true);
                }
                else
                {
                    if (newEnemy.x > 0)
                    {
                        newListOfEnemies.Add(newEnemy);
                    }
                }
            }

            enemies = newListOfEnemies;

            if (!isMulti)
            {
                if (playerOneLives == 0)
                {
                    //End.GameOver(score);
                    return 0;
                }
            }
            else
            {
                if (playerOneLives == 0 && playerTwoLives == 0)
                {
                    //End.GameOver(score);
                    return 0;
                }
                else if (playerOneLives == 0) // First player dies - takes parameters of second player --> single player
                {
                    playerOneDied = true;
                    playerOneLives = playerTwoLives;
                    playerTwoLives = 0;
                    firstPlayer.y = secondPlayer.y;
                }
                else if (playerTwoLives == 0)
                {
                    playerTwoDied = true;
                }
            }
            return 1;
        }

        static void ClearAllObjects(bool isMulti = false)
        {
            if (playerOneLives == 0)
            {
                Console.Beep(100, 900);
            }
            else if (playerTwoLives == 0 && isMulti)
            {
                Console.Beep(100, 900);
            }
            else 
            {
                Console.Beep(300, 300);
            }
            enemies.Clear();
            shots.Clear();
            rocks.Clear();
            bonuses.Clear();
        }

        static int MoveRocks(bool isMulti, bool isOver = false)
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
                    playerOneLives--;
                    ClearAllObjects();
                }
                else if (isMulti && (newRock.x <= secondPlayer.x + secondPlayer.str.Length && newRock.y == secondPlayer.y))
                {
                    playerTwoLives--;
                    ClearAllObjects(true);
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

            if (!isMulti)
            {
                if (playerOneLives == 0)
                {
                    return 0;
                }
            }
            else
            {
                if (playerOneLives == 0 && playerTwoLives == 0)
                {
                    return 0;
                }
                else if (playerOneLives == 0) // First player dies - takes parameters of second player --> single player
                {
                    playerOneDied = true;
                    playerOneLives = playerTwoLives;
                    playerTwoLives = 0;
                    firstPlayer.y = secondPlayer.y;
                }
                else if (playerTwoLives == 0)
                {
                    playerTwoDied = true;
                }
            }
            return 1;
        }

        static void MoveBonusObject(bool isMulti)
        {
            List<Object> newListOfBonus = new List<Object>();
            for (int i = 0; i < bonuses.Count; i++)
            {
                Object oldBonus = bonuses[i];
                Object newBonus = new Object();
                newBonus.x = oldBonus.x - 1;
                newBonus.y = oldBonus.y;
                newBonus.str = oldBonus.str;
                newBonus.color = oldBonus.color;
                //"♥", "●"
                if (newBonus.x <= firstPlayer.x + firstPlayer.str.Length && newBonus.y == firstPlayer.y)
                {
                    if (newBonus.str == "♥")
                    {
                        if (playerOneLives < 5)
                            playerOneLives++;
                    }
                    else if (newBonus.str == "●")
                        playerOneAmmo = 10;
                }
                else if (isMulti && (newBonus.x <= secondPlayer.x + secondPlayer.str.Length && newBonus.y == secondPlayer.y))
                {
                    if (newBonus.str == "♥")
                    {
                        if (playerTwoLives < 5)
                            playerTwoLives++;
                    }
                    else if (newBonus.str == "●")
                        playerOneAmmo = 10;
                }
                else
                {
                    if (newBonus.x > 0)
                    {
                        newListOfBonus.Add(newBonus);
                    }
                }
            }

            bonuses = newListOfBonus;
        }

        static void RedrawPlayfield(bool isMulti = false)
        {
            PrintOnPosition(firstPlayer.x, firstPlayer.y, firstPlayer.str, firstPlayer.color);
            if (isMulti)
            {
                PrintOnPosition(secondPlayer.x, secondPlayer.y, secondPlayer.str, secondPlayer.color);
            }

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
            foreach (Object bonus in bonuses)
            {
                PrintOnPosition(bonus.x, bonus.y, bonus.str, bonus.color);
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
            string line = new string('▄', Console.WindowWidth);
            string livesOne = new string('♥', playerOneLives);
            string livesTwo = new string('♥', playerTwoLives);
            string ammoOne = new string('●', playerOneAmmo);
            string ammoTwo = new string('●', playerTwoAmmo);
            if (playerOneDied)
            {
                PrintOnPosition(Console.WindowWidth - 9, 0, "Player 2", ConsoleColor.Green);
                PrintOnPosition(Console.WindowWidth - playerOneAmmo - 1, 1, ammoOne, ConsoleColor.Green);
                PrintOnPosition(Console.WindowWidth - 7, 2, " Lives", ConsoleColor.White);
                PrintOnPosition(Console.WindowWidth - 12, 2, livesOne, ConsoleColor.Blue);
            }
            else
            {
                PrintOnPosition(1, 0, "Player 1", ConsoleColor.White);
                PrintOnPosition(1, 1, ammoOne, ConsoleColor.White);
                PrintOnPosition(1, 2, "Lives ", ConsoleColor.White);
                PrintOnPosition(7, 2, livesOne, ConsoleColor.Red);
            }


            if (isMulti)
            {
                PrintOnPosition(Console.WindowWidth - 9, 0, "Player 2", ConsoleColor.Green);
                PrintOnPosition(Console.WindowWidth - playerTwoAmmo - 1, 1, ammoTwo, ConsoleColor.Green);
                PrintOnPosition(Console.WindowWidth - 7, 2, " Lives", ConsoleColor.White);
                PrintOnPosition(Console.WindowWidth - 12, 2, livesTwo, ConsoleColor.Blue);
            }

            PrintOnPosition(Console.WindowWidth / 2 - 3, 0, "Level " + currentLevel, ConsoleColor.White);
            PrintOnPosition(Console.WindowWidth / 2 - 3, 1, "Score " + score, ConsoleColor.White);
            PrintOnPosition(0, 3, "" + line, ConsoleColor.White);
        }
    }
}
