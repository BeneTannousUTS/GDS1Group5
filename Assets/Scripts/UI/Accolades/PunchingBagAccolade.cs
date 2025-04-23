using System.Collections.Generic;

public class PunchingBagAccolade : Accolade
{
    public override int SelectBest(List<IndexScoreStat> indexedScoreStats)
    {
        int bestIndex = -1;
        float mostDamageTaken = -1;

        foreach (IndexScoreStat indexScoreStat in indexedScoreStats)
        {
            if (indexScoreStat.scoreStat.damageTaken > mostDamageTaken && indexScoreStat.scoreStat.damageTaken > 500f)
            {
                bestIndex = indexScoreStat.playerIndex;
                mostDamageTaken = indexScoreStat.scoreStat.damageTaken;
            }
        }

        title = "Punching Bag";
        body = $"Took {mostDamageTaken} damage.";

        return bestIndex;
    }
}
