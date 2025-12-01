// AUTHOR: James
// Manages the debug menu
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour
{
    bool debugMode;
    [SerializeField] Canvas debugCanvas;
    [SerializeField] Canvas weaponCanvas;
    [SerializeField] Canvas secondaryCanvas;
    [SerializeField] Canvas passiveCanvas;
    [SerializeField] Canvas enemyCanvas;
    [SerializeField] Canvas dungeonCanvas;
    [SerializeField] Canvas traitorCanvas;
    [SerializeField] Canvas statsCanvas;
    [SerializeField] Canvas cheatsCanvas;
    public GameObject[] cards;


    void debugStart()
    {
        cards = FindAnyObjectByType<CardManager>().cardCanvasPrefab.GetComponentInChildren<CardSelection>().cards;
        foreach (GameObject card in cards)
        {
            Debug.Log(card.name);
        }
        debugMode = !debugMode;
        if (debugMode)
        {
            debugCanvas.gameObject.SetActive(true);
            StartCoroutine(LoadAllCanvas());
        }
        else
        {
            debugCanvas.gameObject.SetActive(false);
        }
    }
    public void LoadWeaponCanvas()
    {
        UnloadAllCanvas();
        weaponCanvas.gameObject.SetActive(true);
    }

    public void LoadSecondaryCanvas()
    {
        UnloadAllCanvas();
        secondaryCanvas.gameObject.SetActive(true);
    }

    public void LoadPassiveCanvas()
    {
        UnloadAllCanvas();
        passiveCanvas.gameObject.SetActive(true);
    }

    public void LoadDungeonCanvas()
    {
        UnloadAllCanvas();
        dungeonCanvas.gameObject.SetActive(true);
    }

    public void LoadEnemyCanvas()
    {
        UnloadAllCanvas();
        enemyCanvas.gameObject.SetActive(true);
    }

    public void LoadTraitorCanvas()
    {
        UnloadAllCanvas();
        traitorCanvas.gameObject.SetActive(true);
    }

    public void LoadStatsCanvas()
    {
        UnloadAllCanvas();
        statsCanvas.gameObject.SetActive(true);
    }

    public void LoadCheatsCanvas()
    {
        UnloadAllCanvas();
        cheatsCanvas.gameObject.SetActive(true);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UnloadAllCanvas()
    {
        weaponCanvas.gameObject.SetActive(false);
        secondaryCanvas.gameObject.SetActive(false);
        passiveCanvas.gameObject.SetActive(false);
        enemyCanvas.gameObject.SetActive(false);
        dungeonCanvas.gameObject.SetActive(false);
        statsCanvas.gameObject.SetActive(false);
        traitorCanvas.gameObject.SetActive(false);
        cheatsCanvas.gameObject.SetActive(false);
    }

    IEnumerator LoadAllCanvas()
    {
        weaponCanvas.gameObject.SetActive(true);
        secondaryCanvas.gameObject.SetActive(true);
        passiveCanvas.gameObject.SetActive(true);
        enemyCanvas.gameObject.SetActive(true);
        dungeonCanvas.gameObject.SetActive(true);
        statsCanvas.gameObject.SetActive(true);
        traitorCanvas.gameObject.SetActive(true);
        cheatsCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.001f);
        weaponCanvas.gameObject.SetActive(false);
        secondaryCanvas.gameObject.SetActive(false);
        passiveCanvas.gameObject.SetActive(false);
        enemyCanvas.gameObject.SetActive(false);
        dungeonCanvas.gameObject.SetActive(false);
        statsCanvas.gameObject.SetActive(false);
        traitorCanvas.gameObject.SetActive(false);
        cheatsCanvas.gameObject.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            debugStart();
        }
    }
}
