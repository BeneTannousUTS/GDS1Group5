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
            if (indexScoreStat.scoreStat.healingGiven > mostHealed && indexScoreStat.scoreStat.healingGiven > 250f)
            {
                bestIndex = indexScoreStat.playerIndex;
                mostHealed = indexScoreStat.scoreStat.healingGiven;
            }
        }

        title = "The Medic";
        body = $"Healed the party {Math.Round(mostHealed)} health.";

        return bestIndex;
    }
}
