using System.Collections.Generic;

public class PunchingBagAccolade : Accolade
{
    public override int SelectBest(List<IndexScoreStat> indexedScoreStats)
    {
        int bestIndex = -1;
        float mostDamageTaken = -1;

        foreach (IndexScoreStat indexScoreStat in indexedScoreStats)
        {
            if (indexScoreStat.scoreStat.damageTaken > mostDamageTaken && indexScoreStat.scoreStat.damageTaken > 300f)
            {
                bestIndex = indexScoreStat.playerIndex;
                mostDamageTaken = indexScoreStat.scoreStat.damageTaken;
            }
        }

        title = "Punching Bag";
        body = $"Took {(int) mostDamageTaken} damage.";

        return bestIndex;
    }
}
