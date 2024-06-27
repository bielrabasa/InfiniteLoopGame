using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardsOnLayers : MonoBehaviour
{
    public List<uint> fullDeck;
    public List<uint> auxDeck;
    public List<uint> toSelect;
    public List<GameObject> toGameDeck;

    public GameObject panelSelectCards;
    public GameObject selectCards;

    public int cardsDraw;

    public bool player_1;

    public uint manaLess;

    void Start()
    {
        //Copy the full deck at the start of the game
        CopyFullDeck();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D) && player_1) DrawCards();
        if(Input.GetKeyDown(KeyCode.A) && !player_1) DrawCards();
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
            uint nextCard;

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
        CardLoader.LoadCards(ref toSelect);
        GameObject cards = GameObject.Find("CardLoader");
        cards.transform.SetParent(selectCards.transform, false);

        foreach (Transform child in cards.transform)
        {
            //Set a new position
            child.transform.localPosition = new Vector3(0, 0, 0f);

            //Add the event PointerUp to select the card
            GameObject theDeck = this.gameObject;

            child.gameObject.AddComponent(typeof(EventTrigger));
            EventTrigger trigger = child.GetComponent<EventTrigger>();

            //Not showed in the inspector, but is there
            EventTrigger.Entry click = new EventTrigger.Entry();
            click.eventID = EventTriggerType.PointerUp;
            click.callback.AddListener(delegate { theDeck.GetComponent<CardsOnLayers>().PreSelectionCards(child.gameObject); });
            trigger.triggers.Add(click);
        }
    }

    public void PreSelectionCards(GameObject selection)
    {
        /*string[] splitArray = selection.name.Split(char.Parse("<"));
        string[] splitName = splitArray[1].Split(char.Parse(">"));
        int number;
        int.TryParse(splitName[0], out number);*/

        //check if the card are in the deck to remove
        for (int i = 0; i < toGameDeck.Count; i++)
        {
           if(selection == toGameDeck[i])
           {
                RemoveToGame(selection);
                manaLess += selection.GetComponent<CardValues>().manaCost;
                return;
           }
        }

        if(selection.GetComponent<CardValues>().manaCost <= manaLess)
        {
            //if its not, add the card
            AddToGame(selection);

            manaLess -= selection.GetComponent<CardValues>().manaCost;
        }
    }

    void AddToGame(GameObject newCard)
    {
        //add the card to the deck
        toGameDeck.Add(newCard);
    }

    void RemoveToGame(GameObject newCard)
    {
        //remove the card to the deck
        toGameDeck.Remove(newCard);
    }

    public void CreateCards()
    {
        //send list toGsmeObject
        MapState.SetCardsOnMap(toGameDeck);
        //destroy cards

        //cards return to the deck
        foreach (Transform child in selectCards.transform)
        {
            Destroy(child.gameObject);
        }

        //Check the selected card to return to the deck or play
        for (int i = toSelect.Count - 1; i >= 0; i--)
        {
            bool toRemoveAndAdd = true;
            for (int j = 0; j < toGameDeck.Count; j++)
            {
                //is selected, so remove from the selection
                if (toSelect[i] == toGameDeck[j].GetComponent<CardValues>().id)
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
