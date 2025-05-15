using System.Collections.Generic;

public class SurvivorAccolade : Accolade
{
    public override int SelectBest(List<IndexScoreStat> indexedScoreStats)
    {
        int bestIndex = -1;
        float mostTimeAlive = -1;

        foreach (IndexScoreStat indexScoreStat in indexedScoreStats)
        {
            if (indexScoreStat.scoreStat.timeAlive > mostTimeAlive && indexScoreStat.scoreStat.timeAlive > 5 * 60)
            {
                bestIndex = indexScoreStat.playerIndex;
                mostTimeAlive = indexScoreStat.scoreStat.timeAlive;
            }
        }

        title = "The Survivor";
        body = $"Stayed alive for {mostTimeAlive / 60} minutes.";

        return bestIndex;
    }
}
