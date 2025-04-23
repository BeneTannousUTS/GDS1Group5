using System.Collections.Generic;

public class SlayerAccolade : Accolade
{
    public override int SelectBest(List<IndexScoreStat> indexedScoreStats)
    {
        int bestIndex = -1;
        int mostKills = -1;

        foreach (IndexScoreStat indexScoreStat in indexedScoreStats)
        {
            if (indexScoreStat.scoreStat.kills > mostKills && indexScoreStat.scoreStat.kills > 50)
            {
                bestIndex = indexScoreStat.playerIndex;
                mostKills = indexScoreStat.scoreStat.kills;
            }
        }

        title = "The Slayer";
        body = $"Killed {mostKills} enemies.";

        return bestIndex;
    }
}
