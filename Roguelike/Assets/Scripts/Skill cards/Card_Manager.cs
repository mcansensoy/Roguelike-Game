using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Manager : MonoBehaviour
{
    public List<Card> cardList;
    public Transform card_location;
    public GameObject cardPrefab;

    private Dictionary<CardCategory, List<Card>> categorizedCards;
    private List<GameObject> spawnedCards = new();


    private bool cardsSpawned = false;


    private void Start()
    {
        if (!cardsSpawned)
        {
            InitializeCards();
            cardsSpawned = true;
        }
    }






    #region Funcs
    public void InitializeCards()
    {
        if (cardsSpawned) return;

        categorizedCards = new Dictionary<CardCategory, List<Card>>();

        foreach (Card card in cardList)
        {
            if (!categorizedCards.ContainsKey(card.category))
            {
                categorizedCards[card.category] = new List<Card>();
            }
            categorizedCards[card.category].Add(card);
        }

        List<CardCategory> categories = new List<CardCategory>(categorizedCards.Keys);
        Shuffle(categories);

        int cardsToSpawn = 3;
        for (int i = 0; i < cardsToSpawn; i++)
        {
            List<Card> cardsInCategory = categorizedCards[categories[i]];
            Shuffle(cardsInCategory);
            SpawnCard(cardsInCategory[0]);
        }

        cardsSpawned = true;
    }







    void SpawnCard(Card card)
    {
        GameObject newCard = Instantiate(cardPrefab, card_location);
        spawnedCards.Add(newCard);

        Card_info cardDisplay = newCard.GetComponent<Card_info>();
        cardDisplay.Initialize(card);
    }





    public void DestroyAllCards()
    {
        foreach (GameObject card in spawnedCards) { Destroy(card); }
        spawnedCards.Clear();
        cardsSpawned = false;
    }


    public void RespawnCards()
    {
        if (cardsSpawned) return;

        DestroyAllCards();
        InitializeCards();
    }



    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    #endregion
}
