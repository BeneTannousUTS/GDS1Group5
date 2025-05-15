using System.Collections.Generic;

public class BoringAccolade : Accolade
{
    public override int SelectBest(List<IndexScoreStat> indexedScoreStats)
    {
        title = "Erm Boring";
        body = $"You did nothing interesting...";

        return -1;
    }
}
