using System.Collections.Generic;

public class JustKindaThereAccolade : Accolade
{
    public override int SelectBest(List<IndexScoreStat> indexedScoreStats)
    {
        IndexScoreStat bestIndexScoreStat = null;
        int lowestScore = int.MaxValue;

        foreach (IndexScoreStat indexScoreStat in indexedScoreStats)
        {
            if (indexScoreStat.scoreStat.score < lowestScore)
            {
                bestIndexScoreStat = indexScoreStat;
                lowestScore = indexScoreStat.scoreStat.score;
            }
        }

        if (bestIndexScoreStat != null)
        {
            if (!bestIndexScoreStat.scoreStat.isTraitor && bestIndexScoreStat.scoreStat.wonGame)
            {
                title = "Just Kinda There";
                body = $"Won with the lowest score.";
                return bestIndexScoreStat.playerIndex;
            }
        }
        return -1;
    }
}
