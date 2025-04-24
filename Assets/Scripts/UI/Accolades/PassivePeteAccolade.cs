using System.Collections.Generic;

public class PassivePeteAccolade : Accolade
{
    public override int SelectBest(List<IndexScoreStat> indexedScoreStats)
    {
        int bestIndex = -1;
        int mostPassivesTaken = -1;

        foreach (IndexScoreStat indexScoreStat in indexedScoreStats)
        {
            if (indexScoreStat.scoreStat.passivesPicked > mostPassivesTaken && indexScoreStat.scoreStat.passivesPicked > 5)
            {
                bestIndex = indexScoreStat.playerIndex;
                mostPassivesTaken = indexScoreStat.scoreStat.passivesPicked;
            }
        }

        title = "Passive Pete";
        body = $"Collected {mostPassivesTaken} passive cards.";

        return bestIndex;
    }
}
