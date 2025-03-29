// AUTHOR: Alistair
// Handles card selection and players getting their abilities

using UnityEngine;
using System.Collections;

public class CardSelection : MonoBehaviour
{
    public Sprite[] passiveCardSprites;
    public Sprite[] secondaryCardSprites;
    public Sprite[] weaponCardSprites;
    public Sprite traitorCardSprite;

    public GameObject[] secondaryComponents;
    public GameObject[] weaponComponents;
    public GameObject[] passiveComponents;

    public GameObject passiveCard;
    public GameObject secondaryCard;
    public GameObject weaponCard;

    private int traitorNum = 3;
    private GameObject[] cardList = new GameObject[4];
    private GameObject[] abilityList = new GameObject[4];
    private int currentIndex = 0;

    // Flips all cards
    public IEnumerator FlipAll()
    {
        yield return new WaitForSeconds(2f);
        foreach (GameObject card in cardList) 
        {
            StartCoroutine(card.GetComponent<CardHandler>().ChangeSprite());
        }
    }

    // Sets the value of final room
    void SetIsFinalRoom(int num) 
    {
        traitorNum = num;
    }

    // Gets the value of ability list
    public GameObject[] GetAbilityList() 
    {
        return abilityList;
    }
    
    // Instantiates a card and sets the item it will be
    void CreateCard(GameObject cardType, Sprite cardFront) 
    {
        cardList[currentIndex] = Instantiate(cardType, transform);
        cardList[currentIndex].GetComponent<CardHandler>().SetFrontSprite(cardFront);
        currentIndex += 1;
    }

    // Handles the randomness for cards and the calls to CreateCard
    void GetRandomCard(bool isTraitor) 
    {
        int cardType = Random.Range(0,3);

        if (isTraitor == true) 
        {
            abilityList[currentIndex] = null;

            if (cardType == 0)
            {
                CreateCard(weaponCard, traitorCardSprite);
            }
            else if (cardType == 1) 
            {
                CreateCard(secondaryCard, traitorCardSprite);
            }
            else 
            {
                CreateCard(passiveCard, traitorCardSprite);
            }
            
        }
        else 
        {
            if (cardType == 0)
            {
                int abilityIndex = Random.Range(0, weaponCardSprites.Length);
                abilityList[currentIndex] = null; //weaponComponents[abilityIndex];
                CreateCard(weaponCard, weaponCardSprites[abilityIndex]);
            }
            else if (cardType == 1) 
            {
                int abilityIndex = Random.Range(0, secondaryCardSprites.Length);
                abilityList[currentIndex] = null; //secondaryComponents[abilityIndex];
                CreateCard(secondaryCard, secondaryCardSprites[abilityIndex]);
            }
            else 
            {
                int abilityIndex = Random.Range(0, passiveCardSprites.Length);
                abilityList[currentIndex] = null; //passiveComponents[abilityIndex];
                CreateCard(passiveCard, passiveCardSprites[abilityIndex]);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (traitorNum == 1) 
        {
            int traitorIndex = Random.Range(0,4);

            GetRandomCard((traitorIndex == 0));
            GetRandomCard((traitorIndex == 1));
            GetRandomCard((traitorIndex == 2));
            GetRandomCard((traitorIndex == 3));
        }
        else if (traitorNum == 3) 
        {
            int traitorIndex = Random.Range(0,4);

            GetRandomCard((traitorIndex != 0));
            GetRandomCard((traitorIndex != 1));
            GetRandomCard((traitorIndex != 2));
            GetRandomCard((traitorIndex != 3));
        }
        else if (traitorNum == 4) 
        {
            GetRandomCard(true);
            GetRandomCard(true);
            GetRandomCard(true);
            GetRandomCard(true);
        }
        else 
        {
            GetRandomCard(false);
            GetRandomCard(false);
            GetRandomCard(false);
            GetRandomCard(false);
        }

        StartCoroutine(FlipAll());
    }
}
