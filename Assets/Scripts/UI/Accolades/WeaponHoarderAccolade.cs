using System.Collections.Generic;

public class WeaponHoarderAccolade : Accolade
{
    public override int SelectBest(List<IndexScoreStat> indexedScoreStats)
    {
        int bestIndex = -1;
        int mostWeaponsTaken = -1;

        foreach (IndexScoreStat indexScoreStat in indexedScoreStats)
        {
            if (indexScoreStat.scoreStat.weaponsPicked > mostWeaponsTaken && indexScoreStat.scoreStat.weaponsPicked > 3)
            {
                bestIndex = indexScoreStat.playerIndex;
                mostWeaponsTaken = indexScoreStat.scoreStat.weaponsPicked;
            }
        }

        title = "Indecisive Killer";
        body = $"Swapped weapons {mostWeaponsTaken} times.";

        return bestIndex;
    }
}
