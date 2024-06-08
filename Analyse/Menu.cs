using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;

namespace ProjectP3
{
    class Menu
    {
        protected List<string> items;
        protected int activeID;
        protected bool activeIDChanged;

        public bool ActiveItemChanged
        { get { return activeIDChanged; } }

        public Menu() 
        {
            items = new List<string>();
            
            items.Add("Play");
            items.Add("HighScores");
            items.Add("Quit");

            activeID = 0;
            activeIDChanged = true;
        }

        public void Draw(int offsetX, int offsetY, int vertSpace)
        {
            Console.Clear();
            for (int i = 0; i < items.Count; i++)
            {
                Console.SetCursorPosition(offsetX, offsetY + i*vertSpace);
                if(i == activeID)//switch colors and write
                {
                    ConsoleColor temp = Console.ForegroundColor;
                    Console.ForegroundColor = Console.BackgroundColor;
                    Console.BackgroundColor = temp;

                    Console.Write(items[i]);

                    Console.BackgroundColor = Console.ForegroundColor;
                    Console.ForegroundColor = temp;
                   
                }
                else//just write it
                {
                    Console.Write(items[i]);
                }
            }
            activeIDChanged = false;
        }

        public void OnInput(ConsoleKey key)
        {
            if (key == ConsoleKey.DownArrow)
            {
               activeID = (activeID + 1) % items.Count;
                activeIDChanged = true;
            }
            if (key == ConsoleKey.UpArrow)
            {
                activeID--;
                if(activeID < 0)
                {
                    activeID = items.Count - 1;
                }
                activeIDChanged = true;
            }
            if (key == ConsoleKey.Enter)
            {
                if (items[activeID] == "Play")
                {
                    Program.state = GameState.play;
                }
                if (items[activeID] == "HighScores")
                {
                    Program.state = GameState.highscores;
                }
                if (items[activeID] == "Quit")
                {
                    Program.state = GameState.quit;
                }
            }
        }


    }
}
