using System;
using System.Collections.Generic;

public class JustASecondAccolade : Accolade
{
    public override int SelectBest(List<IndexScoreStat> indexedScoreStats)
    {
        int bestIndex = -1;
        float mostSecondaryActivations = -1f;

        foreach (IndexScoreStat indexScoreStat in indexedScoreStats)
        {
            if (indexScoreStat.scoreStat.secondaryActivated > mostSecondaryActivations && indexScoreStat.scoreStat.secondaryActivated > 15)
            {
                bestIndex = indexScoreStat.playerIndex;
                mostSecondaryActivations = indexScoreStat.scoreStat.secondaryActivated;
            }
        }

        title = "Just A Second";
        body = $"Used secondary {mostSecondaryActivations} times.";

        return bestIndex;
    }
}
