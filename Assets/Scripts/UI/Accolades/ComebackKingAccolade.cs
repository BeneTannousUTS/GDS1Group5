using System;
using System.Collections.Generic;
using System.Linq;

public class ComebackKingAccolade : Accolade
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

        if (bestIndexScoreStat.scoreStat.isTraitor && bestIndexScoreStat.scoreStat.wonGame)
        {
            title = "Comeback King";
            body = $"Won with the lowest score.";
            return bestIndexScoreStat.playerIndex;
        } else
        {
            return -1;
        }
    }
}
