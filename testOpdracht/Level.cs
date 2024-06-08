using System;
using System.Collections.Generic;

namespace testOpdracht
{
    public class LevelSystem
    {
        private int currentLevel;
        private List<int> roundsCompleted; // Lijst om de voltooide rondes op te slaan
        private int currentRoundHits; // Aantal geraakte schepen in de huidige ronde

        public LevelSystem()
        {
            currentLevel = 1;
            roundsCompleted = new List<int>();
            currentRoundHits = 0;
        }

        public void IncreaseLevel()
        {
            currentLevel++;
            roundsCompleted.Add(currentRoundHits); // Voeg het aantal geraakte schepen in de huidige ronde toe aan de lijst van voltooide rondes
            currentRoundHits = 0; // Reset het aantal geraakte schepen voor de volgende ronde
        }

        public void RecordHit()
        {
            currentRoundHits++; // Verhoog het aantal geraakte schepen in de huidige ronde
        }

        public List<int> GetRoundsCompleted()
        {
            return roundsCompleted;
        }

        public int GetCurrentRoundHits()
        {
            return currentRoundHits;
        }

        public void Reset()
        {
            currentLevel = 1;
            roundsCompleted.Clear();
            currentRoundHits = 0;
        }

        public int GetRemainingTurns()
        {
            // Bepaal het aantal resterende beurten op basis van het huidige niveau
            switch (currentLevel)
            {
                case 1:
                    return 20;
                case 2:
                    return 15;
                case 3:
                    return 12;
                // Voeg hier meer niveaus en beurten toe indien nodig
                default:
                    return 10; // Standaard aantal beurten als het niveau hoger is dan we hebben gedefinieerd
            }
        }
    }
}
