using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardsOnLayers : MonoBehaviour
{
    public List<GameObject> fullDeck;
    public List<GameObject> auxDeck;
    public List<GameObject> toSelect;
    public List<GameObject> gameDeck;

    public GameObject panelSelectCards;
    public GameObject selectCards;

    public int cardsDraw;

    public bool player_1;

    void Start()
    {
        //Copy the full deck at the start of the game
        CopyFullDeck();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D)) DrawCards();
    }

    void DrawCards()
    {
        //Get the cards to play and split from the other cards each turn
        SelectCards();
    }
    
    void CopyFullDeck()
    {
        for (int i = 0; i < fullDeck.Count; i++)
        {
            auxDeck.Add(fullDeck[i]);
        }
    }

    void SelectCards()
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
            toSelect.Add(nextCard);
        }

        //Show a menu to select the cards
        panelSelectCards.SetActive(true);

        //choose between 5 cards
        CreateSelection();
    }

    public void CreateSelection()
    {
        for (int i = 0; i < toSelect.Count; i++)
        {
            //Instatiate the card
            GameObject newCard = Instantiate(toSelect[i]);

            //Set the random layer parent Card
            newCard.transform.SetParent(selectCards.transform, false);

            //Set a new position
            newCard.transform.localPosition = new Vector3((200 * i)-400, 0f, 0f);

            //Add the event PointerUp to select the card
            GameObject theDeck = this.gameObject;

            newCard.AddComponent(typeof(EventTrigger));
            EventTrigger trigger = newCard.GetComponent<EventTrigger>();

            //Not showed in the inspector, but is there
            EventTrigger.Entry click = new EventTrigger.Entry();
            click.eventID = EventTriggerType.PointerUp;
            click.callback.AddListener(delegate { theDeck.GetComponent<CardsOnLayers>().PreSelectionCards(newCard); });
            trigger.triggers.Add(click);
        }
    }

    public void PreSelectionCards(GameObject selection)
    {
        selection = FindOnDeck(selection.name, toSelect);
        //check if the card are in the deck to remove
        for (int i = 0; i < gameDeck.Count; i++)
        {
            if(selection == gameDeck[i])
            {
                RemoveToGame(selection);
                return;
            }
        }

        //if its not, add the card
        AddToGame(selection);
    }

    void AddToGame(GameObject newCard)
    {
        //add the card to the deck
        gameDeck.Add(newCard);
    }

    void RemoveToGame(GameObject newCard)
    {
        //remove the card to the deck
        gameDeck.Remove(newCard);
    }

    GameObject FindOnDeck(string name, List<GameObject> deck)
    {
        //check if the card is a copy or original
        if(name.Contains(char.Parse("(")))
        {
            //get just the name of the card
            string[] splitArray = name.Split(char.Parse("("));
            name = splitArray[0];
        }

        //retun the game object  with the name in the deck
        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i].name == name) return deck[i];
        }
        
        //if the name is not in the deck, return null
        return null;
    }

    public void CreateCards()
    {
        for (int i = 0; i < gameDeck.Count; i++)
        {
            //Instatiate the card
            GameObject newLayer = Instantiate(gameDeck[i]);

            //Get Layer List from LayerGen script
            List<GameObject> layers;
            if (player_1) layers = GameObject.Find("LayersParent").GetComponent<LayersGen>().layerListP1;
            else layers = GameObject.Find("LayersParent").GetComponent<LayersGen>().layerListP0;

            //Get a random Layer from list
            GameObject aux;
            aux = layers[Random.Range(0, layers.Count)];

            //Set the random layer parent Card
            newLayer.transform.SetParent(aux.transform, false);
        }

        //cards return to the deck
        foreach (Transform child in selectCards.transform)
        {
            Destroy(child.gameObject);
        }

        //Check the selected card to return to the deck or play
        for (int i = toSelect.Count - 1; i >= 0; i--)
        {
            bool toRemoveAndAdd = true;
            for (int j = 0; j < gameDeck.Count; j++)
            {
                //is selected, so remove from the selection
                if (toSelect[i] == gameDeck[j])
                {
                    toSelect.Remove(toSelect[i]);
                    toRemoveAndAdd = false;
                    break;
                }
            }
            //is not selected, so returns to the deck and remove from selection
            if(toRemoveAndAdd)
            {
                auxDeck.Add(toSelect[i]);
                toSelect.Remove(toSelect[i]);
            }
        }
    }
}
