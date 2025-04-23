using System.Collections.Generic;
using UnityEngine;

public abstract class Accolade
{
    public string title;
    public string body;

    public abstract int SelectBest(List<IndexScoreStat> indexedScoreStats);
}
