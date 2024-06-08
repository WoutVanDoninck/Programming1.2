using System;
using System.IO;
using System.Collections.Generic;
using testOpdracht;

public class Menu
{
    private int _selectedOption;
    private string[] _options;
    private LevelSystem levelSystem;

    public Menu(string[] options)
    {
        _options = options;
        _selectedOption = 0;
    }

    public void DisplayMenu()
    {
        Console.Clear();
        string title =@"
██████╗  █████╗ ████████╗████████╗██╗     ███████╗    ███████╗██╗  ██╗██╗██████╗ ███████╗
██╔══██╗██╔══██╗╚══██╔══╝╚══██╔══╝██║     ██╔════╝    ██╔════╝██║  ██║██║██╔══██╗██╔════╝
██████╔╝███████║   ██║      ██║   ██║     █████╗      ███████╗███████║██║██████╔╝███████╗
██╔══██╗██╔══██║   ██║      ██║   ██║     ██╔══╝      ╚════██║██╔══██║██║██╔═══╝ ╚════██║
██████╔╝██║  ██║   ██║      ██║   ███████╗███████╗    ███████║██║  ██║██║██║     ███████║
╚═════╝ ╚═╝  ╚═╝   ╚═╝      ╚═╝   ╚══════╝╚══════╝    ╚══════╝╚═╝  ╚═╝╚═╝╚═╝     ╚══════╝
                                                                                         
";
            Console.WriteLine(title);
        for (int i = 0; i < _options.Length; i++)
        {
            if (i == _selectedOption)
            {
                Console.WriteLine($"=> {_options[i]}");
            }
            else
            {
                Console.WriteLine($"  {_options[i]}");
            }
        }
    }

    public void HandleInput(ConsoleKeyInfo keyInfo)
    {
        switch (keyInfo.Key)
        {
            case ConsoleKey.UpArrow:
                _selectedOption = (_selectedOption - 1 + _options.Length) % _options.Length;
                break;
            case ConsoleKey.DownArrow:
                _selectedOption = (_selectedOption + 1) % _options.Length;
                break;
            case ConsoleKey.Enter:
                SelectOption();
                break;
        }
    }

    private void SelectOption()
    {
        switch (_selectedOption)
        {
            case 0: // Play Game
                Console.Clear();
                
                Board board = new Board(7, 7);
                int score = board.PlayGame();
                Console.Write("Enter your name:");
                string name = Console.ReadLine();
                SaveHighScore(name, score);
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey(true);
                break;
            case 1: // High Scores
                Console.Clear();
                string title =@"
██╗  ██╗██╗ ██████╗ ██╗  ██╗    ███████╗ ██████╗ ██████╗ ██████╗ ███████╗███████╗
██║  ██║██║██╔════╝ ██║  ██║    ██╔════╝██╔════╝██╔═══██╗██╔══██╗██╔════╝██╔════╝
███████║██║██║  ███╗███████║    ███████╗██║     ██║   ██║██████╔╝█████╗  ███████╗
██╔══██║██║██║   ██║██╔══██║    ╚════██║██║     ██║   ██║██╔══██╗██╔══╝  ╚════██║
██║  ██║██║╚██████╔╝██║  ██║    ███████║╚██████╗╚██████╔╝██║  ██║███████╗███████║
╚═╝  ╚═╝╚═╝ ╚═════╝ ╚═╝  ╚═╝    ╚══════╝ ╚═════╝ ╚═════╝ ╚═╝  ╚═╝╚══════╝╚══════╝
                                                                                 
";
                Console.WriteLine(title);
                string[] highScores = LoadHighScores();
                foreach (string point in highScores)
                {
                    Console.WriteLine(point);
                }
                Console.WriteLine("Press Escape to return to the menu...");
                while (true)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        return;
                    }
                }
                break;
            case 2: // Exit
                Environment.Exit(0);
                break;
        }
    }

    private string[] LoadHighScores()
    {
        string[] highScores = new string[0];
        if (File.Exists("highscores.txt"))
        {
            highScores = File.ReadAllLines("highscores.txt");
            Array.Sort(highScores, (a, b) => int.Parse(b.Split('-')[1].Trim()).CompareTo(int.Parse(a.Split('-')[1].Trim())));
        }
        return highScores;
    }

    public void SaveHighScore(string name, int score)
{
    string[] highScores = LoadHighScores();
    string newScore = $"{name} - {score}";
    highScores = AddScoreToHighScores(highScores, newScore);
    if (highScores.Length > 5)
    {
        Array.Sort(highScores, (a, b) => int.Parse(b.Split('-')[1].Trim()).CompareTo(int.Parse(a.Split('-')[1].Trim())));
        highScores = highScores.Take(5).ToArray();
    }
    File.WriteAllLines("highscores.txt", highScores);
}

    private string[] AddScoreToHighScores(string[] highScores, string newScore)
    {
        // Implement logic to add the new score to the high scores array
        // You can sort the array by score and limit it to a certain number of entries
        // For now, just add the new score to the end of the array
        string[] newHighScores = new string[highScores.Length + 1];
        highScores.CopyTo(newHighScores, 0);
        newHighScores[highScores.Length] = newScore;
        return newHighScores;
    }
}