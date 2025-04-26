using System.Collections;
using System.Collections.Generic;
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
    GameObject readyPlayerPrefab;
    [SerializeField]
    GameObject readyTextGroup;
    private List<BaseConfirmHandler> readyCheckHandlers = new();

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
        StartCoroutine(ReadyCheckSequence(players));
    }

        IEnumerator ReadyCheckSequence(PlayerData[] players)
    {
        SetPlayerPrefabs(players);
        yield return ConfirmManager.Instance.WaitForAllConfirmations(readyCheckHandlers);

        Debug.Log("All players confirmed traitor");

        yield return new WaitForSeconds(0.25f);

        DestroyTraitorCanvas();
    }

    private void SetPlayerPrefabs(PlayerData[] players)
    {
        foreach (PlayerData playerData in players)
        {
            if (!playerData.isJoined) continue;

            GameObject readyCheckObject = Instantiate(readyPlayerPrefab, readyCheckCanvas.transform);
            ReadyCheckHandler handler = readyCheckObject.GetComponent<ReadyCheckHandler>();

            handler.init(playerData.playerInput, playerData.playerIndex);
            handler.Setup(PlayerManager.instance.playerSprites[playerData.playerIndex]);

            readyCheckHandlers.Add(handler);
        }
    }

    public void DestroyTraitorCanvas()
    {
        FindAnyObjectByType<CardSelection>().ResumeGameplay();
        Destroy(gameObject);
    }
}
