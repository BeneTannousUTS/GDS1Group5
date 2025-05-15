using System;
using System.Collections.Generic;

public class ZoomerAccolade : Accolade
{
    public override int SelectBest(List<IndexScoreStat> indexedScoreStats)
    {
        int bestIndex = -1;
        float topSpeed = -1f;

        foreach (IndexScoreStat indexScoreStat in indexedScoreStats)
        {
            if (indexScoreStat.scoreStat.topSpeed > topSpeed && indexScoreStat.scoreStat.topSpeed > 10.0f)
            {
                bestIndex = indexScoreStat.playerIndex;
                topSpeed = indexScoreStat.scoreStat.topSpeed;
            }
        }

        title = "The Zoomer";
        body = $"Ran around at {Math.Round(topSpeed * 3.6f, 1)}km/h."; // 3.6 to convert from m/s to km/h

        return bestIndex;
    }
}
