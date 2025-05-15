using System;
using System.Collections.Generic;

public class OuchAccolade : Accolade
{
    public override int SelectBest(List<IndexScoreStat> indexedScoreStats)
    {
        int bestIndex = -1;
        int mostTraps = -1;

        foreach (IndexScoreStat indexScoreStat in indexedScoreStats)
        {
            if (indexScoreStat.scoreStat.hazardsRanInto > mostTraps && indexScoreStat.scoreStat.hazardsRanInto > 10)
            {
                bestIndex = indexScoreStat.playerIndex;
                mostTraps = indexScoreStat.scoreStat.hazardsRanInto;
            }
        }

        title = "Sore Feet";
        body = $"Ran over {mostTraps} traps.";

        return bestIndex;
    }
}
