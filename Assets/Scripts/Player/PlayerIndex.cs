using UnityEngine;

public class PlayerIndex : MonoBehaviour
{
    public int playerIndex;
    public bool isOccupied;

    public void SetOccupied(bool occupied)
    {
        isOccupied = occupied;
    }
}
