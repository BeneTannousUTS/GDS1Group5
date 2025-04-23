using System;
using System.Collections.Generic;

public class DontMissAccolade : Accolade
{
    public override int SelectBest(List<IndexScoreStat> indexedScoreStats)
    {
        int bestIndex = -1;
        float bestAccuracy = -1;

        foreach (IndexScoreStat indexScoreStat in indexedScoreStats)
        {
            float accuracy = (float) indexScoreStat.scoreStat.projectilesHit / (float) indexScoreStat.scoreStat.projectilesShot;

            if (accuracy > bestAccuracy && accuracy > 0.9f)
            {
                bestIndex = indexScoreStat.playerIndex;
                bestAccuracy = accuracy;
            }
        }

        title = "Sharp Shooter";
        body = $"Shot with {Math.Min(Math.Round(bestAccuracy * 100), 99)}% accurracy.";

        return bestIndex;
    }
}
