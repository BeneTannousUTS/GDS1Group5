using System;
using System.Collections.Generic;

public class SpammerAccolade : Accolade
{
    public override int SelectBest(List<IndexScoreStat> indexedScoreStats)
    {
        int bestIndex = -1;
        int mostWeaponActivations = -1;

        foreach (IndexScoreStat indexScoreStat in indexedScoreStats)
        {
            if (indexScoreStat.scoreStat.weaponActivated > mostWeaponActivations && indexScoreStat.scoreStat.weaponActivated > 50)
            {
                bestIndex = indexScoreStat.playerIndex;
                mostWeaponActivations = indexScoreStat.scoreStat.weaponActivated;
            }
        }

        title = "SPAMMER";
        body = $"Used weapon {mostWeaponActivations} times.";

        return bestIndex;
    }
}
