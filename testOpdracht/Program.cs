using System;

namespace testOpdracht
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Menu menu = new Menu(new string[] { "Play Game", "High Scores", "Exit" });
            while (true)
            {
                menu.DisplayMenu();
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                menu.HandleInput(keyInfo);
            }
        }
    }
}