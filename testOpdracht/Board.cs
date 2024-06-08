using System;
using System.Collections.Generic;

namespace testOpdracht
{
    public class Board
    {
        private char[,] grid;
        private bool[,] hits;
        private int numRows;
        private int numCols;
        private Player player;
        private bool[,] selectedCells; // Maximaal aantal beurten
        private LevelSystem levelSystem;

        public Board(int numRows, int numCols) 
        {
            this.numRows = numRows;
            this.numCols = numCols;
            InitializeGrid();
            PlaceShips();
            selectedCells = new bool[numRows, numCols]; // Initialiseer geraakte cellen
            player = new Player(numRows, numCols);
            levelSystem = new LevelSystem(); // Maak een instantie van de LevelSystem klasse
        }

        private void InitializeGrid()
        {
            grid = new char[numRows, numCols];
            hits = new bool[numRows, numCols];

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    grid[i, j] = '.';
                    hits[i, j] = false;
                }
            }
        }

        public void PlaceShips()
        {
            // Plaats twee boten van elk drie cellen lang
            PlaceShip(3);
            PlaceShip(3);
        }

        private void PlaceShip(int length)
        {
            Random random = new Random();
            int direction = random.Next(2); // 0 voor horizontaal, 1 voor verticaal

            int row, col;

            if (direction == 0) // Horizontaal
            {
                row = random.Next(numRows);
                col = random.Next(numCols - length + 1);

                for (int j = col; j < col + length; j++)
                {
                    grid[row, j] = '@';
                }
            }
            else // Verticaal
            {
                row = random.Next(numRows - length + 1);
                col = random.Next(numCols);

                for (int i = row; i < row + length; i++)
                {
                    grid[i, col] = '@';
                }
            }
        }

        public int PlayGame()
        {
            bool playAgain = true;
            int score =0 ;
            

            while (playAgain)
            {
                
                player.remainingTurns = levelSystem.GetRemainingTurns(); // Ontvang het aantal resterende beurten van de levelSystem

                while (player.remainingTurns > 0)
                {
                    // Afdrukken van het bord en aantal resterende zetten
                    PrintBoard();
                    Console.WriteLine($"Remaining Turns: {player.remainingTurns}");

                    Console.WriteLine("Use arrow keys to select a cell. Press Enter to confirm.");

                    // Gebruikersinvoer verwerken
                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    int prevPlayerRow = player.Row;
                    int prevPlayerCol = player.Col;

                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        // Controleer of de geselecteerde cel al is geraakt
                        if (selectedCells[player.Row, player.Col])
                        {
                            Console.WriteLine("You already hit this cell.");
                            continue;
                        }

                        // Controleren of de geselecteerde cel een schip bevat
                        if (grid[player.Row, player.Col] == '@')
                        {
                            Console.WriteLine("Hit!");
                            hits[player.Row, player.Col] = true;
                            selectedCells[player.Row, player.Col] = true; // Markeer de cel als geraakt

                            levelSystem.RecordHit(); // Record de hit in het LevelSystem
                            score += levelSystem.GetCurrentRoundHits();
                            // Controleren of alle boten zijn vernietigd
                            if (AllShipsDestroyed())
                            {
                                levelSystem.IncreaseLevel();
                                
                                Console.Clear();
                                string nxt =@"
███╗   ██╗███████╗██╗  ██╗████████╗    ██████╗  ██████╗ ██╗   ██╗███╗   ██╗██████╗ ██████╗ 
████╗  ██║██╔════╝╚██╗██╔╝╚══██╔══╝    ██╔══██╗██╔═══██╗██║   ██║████╗  ██║██╔══██╗╚════██╗
██╔██╗ ██║█████╗   ╚███╔╝    ██║       ██████╔╝██║   ██║██║   ██║██╔██╗ ██║██║  ██║  ▄███╔╝
██║╚██╗██║██╔══╝   ██╔██╗    ██║       ██╔══██╗██║   ██║██║   ██║██║╚██╗██║██║  ██║  ▀▀══╝ 
██║ ╚████║███████╗██╔╝ ██╗   ██║       ██║  ██║╚██████╔╝╚██████╔╝██║ ╚████║██████╔╝  ██╗   
╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝   ╚═╝       ╚═╝  ╚═╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═══╝╚═════╝   ╚═╝   
                                                                                           
";
                                Console.WriteLine(nxt);
                                
                                Console.WriteLine("Congratulations! You win!");
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Miss!");
                            grid[player.Row, player.Col] = 'O';
                            selectedCells[player.Row, player.Col] = true; // Markeer de cel als geraakt
                        }
                        // Verminder het aantal resterende beurten
                        player.DecreaseTurn();
                    }
                    else if (keyInfo.Key == ConsoleKey.UpArrow)
                    {
                        if (player.Row > 0)
                            player.Row--;
                    }
                    else if (keyInfo.Key == ConsoleKey.DownArrow)
                    {
                        if (player.Row < numRows - 1)
                            player.Row++;
                    }
                    else if (keyInfo.Key == ConsoleKey.LeftArrow)
                    {
                        if (player.Col > 0)
                            player.Col--;
                    }
                    else if (keyInfo.Key == ConsoleKey.RightArrow)
                    {
                        if (player.Col < numCols - 1)
                            player.Col++;
                    }

                    // Als de speler dezelfde cel heeft geselecteerd, verlies geen beurten
                    if (prevPlayerRow == player.Row && prevPlayerCol == player.Col)
                    {
                        Console.WriteLine("You already hit this cell.");
                    }

                    Console.Clear();
                }

                if (player.remainingTurns == 0)
                {
                    string GameOver =@"
 ██████╗  █████╗ ███╗   ███╗███████╗     ██████╗ ██╗   ██╗███████╗██████╗ 
██╔════╝ ██╔══██╗████╗ ████║██╔════╝    ██╔═══██╗██║   ██║██╔════╝██╔══██╗
██║  ███╗███████║██╔████╔██║█████╗      ██║   ██║██║   ██║█████╗  ██████╔╝
██║   ██║██╔══██║██║╚██╔╝██║██╔══╝      ██║   ██║╚██╗ ██╔╝██╔══╝  ██╔══██╗
╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗    ╚██████╔╝ ╚████╔╝ ███████╗██║  ██║
 ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝     ╚═════╝   ╚═══╝  ╚══════╝╚═╝  ╚═╝
                                                                          
";
                    Console.WriteLine(GameOver);
                    Console.WriteLine("You've run out of turns.");
                    
                }

                char response;
                do
                {
                    Console.Write("Do you want to play again? (Y/N): ");
                    response = Console.ReadKey().KeyChar;
                    Console.WriteLine();
                } while (response != 'Y' && response != 'y' && response != 'N' && response != 'n');

                if (response != 'Y' && response != 'y'){
                    playAgain = false;
                    return score;
                }
                else
                {
                    // Reset het spel voor een nieuwe ronde
                    InitializeGrid();
                    PlaceShips();
                    player.Row = 0;
                    player.Col = 0;
                    player.remainingTurns = 20;
                    selectedCells = new bool[numRows, numCols]; // Reset de geraakte cellen
                }
            }
            return score;
        }

        private void PrintBoard()
        {
            Console.Clear(); // Wis het consolevenster voordat het bord wordt afgedrukt
            string BS =@"
██████╗  █████╗ ████████╗████████╗██╗     ███████╗    ███████╗██╗  ██╗██╗██████╗ ███████╗
██╔══██╗██╔══██╗╚══██╔══╝╚══██╔══╝██║     ██╔════╝    ██╔════╝██║  ██║██║██╔══██╗██╔════╝
██████╔╝███████║   ██║      ██║   ██║     █████╗      ███████╗███████║██║██████╔╝███████╗
██╔══██╗██╔══██║   ██║      ██║   ██║     ██╔══╝      ╚════██║██╔══██║██║██╔═══╝ ╚════██║
██████╔╝██║  ██║   ██║      ██║   ███████╗███████╗    ███████║██║  ██║██║██║     ███████║
╚═════╝ ╚═╝  ╚═╝   ╚═╝      ╚═╝   ╚══════╝╚══════╝    ╚══════╝╚═╝  ╚═╝╚═╝╚═╝     ╚══════╝
                                                                                         
";
            Console.WriteLine(BS);
            // Header met kolomnummers
            Console.Write("  ");
            for (int i = 0; i < numCols; i++)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            // Afdrukken van het bord
            for (int i = 0; i < numRows; i++)
            {
                Console.Write(i + " "); // Rijnummer
                for (int j = 0; j < numCols; j++)
                {
                    char cellSymbol = '.';

                    // Controleer of de cel een schip bevat of is geraakt
                    if (hits[i, j])
                    {
                        cellSymbol = '@'; // Geraakte cel
                    }
                    else if (selectedCells[i, j])
                    {
                        cellSymbol = 'O'; // Gemiste cel
                    }
                    else if (i == player.Row && j == player.Col)
                    {
                        cellSymbol = 'X'; // Huidige positie van de speler
                    }

                    Console.Write(cellSymbol + " ");
                }
                Console.WriteLine();
            }

            // Afdrukken van het scorebord
            Console.WriteLine("Scoreboard:");
            foreach (int roundHits in levelSystem.GetRoundsCompleted())
            {
                Console.WriteLine("Ronde voltooid met hits: " + roundHits);
            }

            // Afdrukken van het aantal geraakte schepen in de huidige ronde
            Console.WriteLine("Score: " + levelSystem.GetCurrentRoundHits());
        }

        private bool AllShipsDestroyed()
        {
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    if (grid[i, j] == '@' && !hits[i, j])
                        return false;
                }
            }
            return true;
        }
    }
}
