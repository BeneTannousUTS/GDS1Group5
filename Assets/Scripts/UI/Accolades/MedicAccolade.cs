using System;
using System.Collections.Generic;

public class MedicAccolade : Accolade
{
    public override int SelectBest(List<IndexScoreStat> indexedScoreStats)
    {
        int bestIndex = -1;
        float mostHealed = -1;

        foreach (IndexScoreStat indexScoreStat in indexedScoreStats)
        {
            if (indexScoreStat.scoreStat.healingGiven > mostHealed && indexScoreStat.scoreStat.healingGiven > 100f)
            {
                bestIndex = indexScoreStat.playerIndex;
                mostHealed = indexScoreStat.scoreStat.healingGiven;
            }
        }

        title = "The Medic";
        body = $"Healed the party {(int) Math.Round(mostHealed)} health.";

        return bestIndex;
    }
}
