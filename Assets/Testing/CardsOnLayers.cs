using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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
    public uint maxMana;
    public uint turn = 1;

    int spacesLeft;

    void Start()
    {
        //Copy the full deck at the start of the game
        CopyFullDeck();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D) && player_1) DrawCards();
        if(Input.GetKeyDown(KeyCode.A) && !player_1) DrawCards();
        if(Input.GetKeyDown(KeyCode.S)) turn++;
    }

    void DrawCards()
    {
        //Add the new mana (the turn number)
        manaLess += turn;
        //set 10 as max Mana
        if (manaLess > maxMana) manaLess = maxMana;

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

        //Set the text to show the mana have less
        SetMana();

        //choose between 5 cards
        CreateSelection();
    }

    public void CreateSelection()
    {
        CardLoader.LoadCards(ref toSelect);
        GameObject cards = GameObject.Find("CardLoader");
        cards.transform.SetParent(selectCards.transform, false);

        int i = 0;

        foreach (Transform child in cards.transform)
        {
            //Set a new position
            child.transform.localPosition = new Vector3((350 * i) - (175 * (cards.transform.childCount - 1)), 100.0f, 0.0f);

            //Add the event PointerUp to select the card
            GameObject theDeck = this.gameObject;

            child.gameObject.AddComponent(typeof(EventTrigger));
            EventTrigger trigger = child.GetComponent<EventTrigger>();

            //Not showed in the inspector, but is there
            EventTrigger.Entry click = new EventTrigger.Entry();
            click.eventID = EventTriggerType.PointerUp;
            click.callback.AddListener(delegate { theDeck.GetComponent<CardsOnLayers>().PreSelectionCards(child.gameObject); });
            trigger.triggers.Add(click);

            i++;
        }

        spacesLeft = MapState.SpacesLeft();
    }

    public void PreSelectionCards(GameObject selection)
    {
        //check if the card are in the deck to remove
        for (int i = 0; i < toGameDeck.Count; i++)
        {
           if(selection == toGameDeck[i])
           {
                RemoveToGame(selection);
                return;
           }
        }

        //if (selection.GetComponent<CardValues>().manaCost <= manaLess && spacesLeft < 0)
        if (selection.GetComponent<CardValues>().manaCost <= manaLess)
        {
            //if its not, add the card
            AddToGame(selection);
        }
    }

    void AddToGame(GameObject newCard)
    {
        //add the card to the deck
        toGameDeck.Add(newCard);
        //subtract the mana
        manaLess -= newCard.GetComponent<CardValues>().manaCost;
        //-1 space in the pool
        spacesLeft--;

        //Set the text to show the mana have less
        SetMana();
        //set a new color for the card (glowing)
        newCard.transform.Find("CardImage").GetComponent<Image>().color = new Color(0.98f, 1.0f, 0.80f, 1.0f);
    }

    void RemoveToGame(GameObject newCard)
    {
        //remove the card to the deck
        toGameDeck.Remove(newCard);
        //take back the mana
        manaLess += newCard.GetComponent<CardValues>().manaCost;
        //take back the space in the pool
        spacesLeft++;

        //Set the text to show the mana have less
        SetMana();
        //set athe original color
        newCard.transform.Find("CardImage").GetComponent<Image>().color = Color.white;
    }

    public void CreateCards()
    {
        //send list toGsmeObject
        MapState.SetCardsOnMap(toGameDeck);

        //destroy physical cards
        Destroy(selectCards.transform.GetChild(0).gameObject);

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
        toGameDeck.Clear();
    }

    void SetMana()
    {
        panelSelectCards.transform.Find("Mana").Find("Mana_Text").GetComponent<TMP_Text>().text = manaLess.ToString();
    }
}
