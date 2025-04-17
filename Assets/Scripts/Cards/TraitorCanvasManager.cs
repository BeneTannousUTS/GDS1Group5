using TMPro;
using UnityEngine;

public class TraitorCanvasManager : MonoBehaviour
{
    public TMP_Text traitorName;
    public TMP_Text traitorDesc;
    public TMP_Text abilityName;
    public TMP_Text abilityDesc;
    [SerializeField]
    GameObject readyCheckCanvas;
    [SerializeField]
    GameObject readyTextGroup;

    public void SetTraitorType(BaseTraitor traitorType)
    {
        traitorName.text = traitorType.GetTraitorName();
        traitorDesc.text = traitorType.GetTraitorDesc();
        abilityName.text = traitorType.GetAbilityName();
        abilityDesc.text = traitorType.GetAbilityDesc();
    }

    public void StartReadyCheck(PlayerData[] players)
    {
        readyTextGroup.SetActive(true);
        readyCheckCanvas.GetComponent<ReadyCanvasManager>().StartReadyCheck(players);
    }

    public void DestroyTraitorCanvas()
    {
        FindAnyObjectByType<CardSelection>().ResumeGameplay();
        Destroy(gameObject);
    }
}
