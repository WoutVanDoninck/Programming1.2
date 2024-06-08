using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectP3
{
    class HighScores
    {
        protected static List<int> scores;
        protected static string filePath;
        protected static int maxHighScores = 10;

        public HighScores(string filename, int newMaxHighScores)
        {
            filePath = filename;
            LoadFromFile();
            maxHighScores = newMaxHighScores;
        }

        public void LoadFromFile()
        {
            scores = new List<int>();

            try
            {
                using (StreamReader file = new StreamReader(filePath))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        scores.Add(int.Parse(line));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            scores.Sort();
            scores.Reverse();
            while (scores.Count > maxHighScores)
            {
                scores.RemoveAt(scores.Count - 1);
            }
        }

        public static void SaveHighScores()
        {
            using (StreamWriter file = new StreamWriter(filePath))
            {
                foreach (int i in scores)
                {
                    file.WriteLine(i);
                }
            }
        }

        public static void AddHighScore(int value)
        {
            scores.Add(value);
            scores.Sort();
            scores.Reverse();
            while (scores.Count > maxHighScores)
            {
                scores.RemoveAt(scores.Count - 1);
            }
            SaveHighScores();
        }

        public void Display()
        {
            LoadFromFile();

            Console.Clear();

            Console.SetCursorPosition(10, 10);
            Console.Write("HIGHSCORES");
            Console.SetCursorPosition(10, 11);
            Console.Write("==========");

            for (int i = 0; i < scores.Count; i++)
            {
                Console.SetCursorPosition(10, 12 + i);
                if (i % 2 == 0) // switch colors and write
                {
                    ConsoleColor temp = Console.ForegroundColor;
                    Console.ForegroundColor = Console.BackgroundColor;
                    Console.BackgroundColor = temp;

                    Console.Write(scores[i]);

                    Console.BackgroundColor = Console.ForegroundColor;
                    Console.ForegroundColor = temp;
                }
                else // just write it
                {
                    Console.Write(scores[i]);
                }
            }

            Console.SetCursorPosition(2, 12 + scores.Count);
            Console.Write("Press ESCAPE to go back to menu.");
            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
            Program.state = GameState.menu;
        }
    }
}