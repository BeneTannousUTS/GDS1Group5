using System.Collections.Generic;

public class CasperAccolade : Accolade
{
    public override int SelectBest(List<IndexScoreStat> indexedScoreStats)
    {
        int bestIndex = -1;
        int mostDeaths = -1;

        foreach (IndexScoreStat indexScoreStat in indexedScoreStats)
        {
            if (indexScoreStat.scoreStat.deaths > mostDeaths && indexScoreStat.scoreStat.deaths > 5)
            {
                bestIndex = indexScoreStat.playerIndex;
                mostDeaths = indexScoreStat.scoreStat.deaths;
            }
        }

        title = "Casper The Ghost";
        body = $"Died {mostDeaths} times.";

        return bestIndex;
    }
}
