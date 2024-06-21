using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsOnLayers : MonoBehaviour
{
    public List<GameObject> fullDeck;
    public List<GameObject> auxDeck;
    public List<GameObject> gameDeck;

    public int cardsDraw;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D)) DrawCards();
    }

    void DrawCards()
    {
        //Copy the full deck
        CopyFullDeck();

        //Get the cards to play and split from the other cards
        SplitDeck();

        //Instatiate cards
        CreateCards();
    }
    
    void CopyFullDeck()
    {
        for (int i = 0; i < fullDeck.Count; i++)
        {
            auxDeck.Add(fullDeck[i]);
        }
    }

    void SplitDeck()
    {
        if(cardsDraw > auxDeck.Count) cardsDraw = auxDeck.Count;

        for (int i = 0; i < cardsDraw; i++)
        {
            GameObject nextCard;

            //Get Random Card from the auxiliar deck
            nextCard = auxDeck[Random.Range(0, auxDeck.Count)];

            //Remove that card from the auxiliar deck
            auxDeck.Remove(nextCard);

            //Add that card to the deck for playing
            gameDeck.Add(nextCard);
        }
    }

    void CreateCards()
    {
        for (int i = 0; i < gameDeck.Count; i++)
        {
            //Instatiate the card
            GameObject newLayer = Instantiate(gameDeck[i]);

            //Get Layer List from LayerGen script
            LayersGen layers = GameObject.Find("LayersParent").GetComponent<LayersGen>();

            //Get a random Layer from list
            GameObject aux;
            aux = layers.layerList[Random.Range(0, layers.layerList.Count)];

            //Set the random layer parent Card
            newLayer.transform.SetParent(aux.transform, false);
        }
    }
}
