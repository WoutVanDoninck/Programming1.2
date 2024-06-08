using System;
namespace testOpdracht
{
    public class Player
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int remainingTurns { get; set; }

        public Player(int numRows, int numCols)
        {
            Row = 0;
            Col = 0;
            remainingTurns = 20; // Begin met 20 beurten
        }

        public void DecreaseTurn()
        {
            remainingTurns--;
        }
    }
}

