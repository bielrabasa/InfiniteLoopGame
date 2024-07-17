using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DESIGN;

public class CardsOnLayers : MonoBehaviour
{
    [SerializeField] LayerMask cardLayer;
    [SerializeField] GameObject panelSelectCards;

    int shownCardsNumber = 5;

    int currentPlayerMana = -1;
    int currentSpacesLeft = -1;

    //Players full decks
    List<uint> bottomPlayerDeck = new List<uint>();
    List<uint> topPlayerDeck = new List<uint>();

    //Cards shown in screen
    List<uint> shownCards = new List<uint>();

    //Selected cards
    List<GameObject> selectedCards = new List<GameObject>();

    //Needs to stop animating?
    bool stopAnimating = false;

    void Start()
    {
        for (int i = 0; i < CardLoader.GetDeckSize(); i++)
        {
            bottomPlayerDeck.Add((uint)(i + 1));
            topPlayerDeck.Add((uint)(i + 1));
        }
    }

    void Update()
    {
        if (MapState.turnPhase != MapState.TurnPhase.CARD_SELECTING) return;

        DetectCard();
    }

    //On Draw Cards
    public void DrawCards()
    {
        stopAnimating = false;

        //Set current mana & pick cards from player deck

        if (MapState.bottomPlayerAtacking)
        {
            currentPlayerMana = MapState.bottomMana;
            DrawCardsFromDeck(ref bottomPlayerDeck);
        }
        else
        {
            currentPlayerMana = MapState.topMana;
            DrawCardsFromDeck(ref topPlayerDeck);
        }
    }

    void DrawCardsFromDeck(ref List<uint> currentPlayerDeck)
    {
        if(shownCardsNumber > currentPlayerDeck.Count) shownCardsNumber = currentPlayerDeck.Count;

        List<uint> temporalDeckCopy = new List<uint>(currentPlayerDeck);
        for (int i = 0; i < shownCardsNumber; i++)
        {
            uint nextCard = temporalDeckCopy[Random.Range(0, temporalDeckCopy.Count)];
            temporalDeckCopy.Remove(nextCard);
            shownCards.Add(nextCard);
        }

        //Show a menu to select the cards
        panelSelectCards.SetActive(true);

        //Set the text to show the mana have less
        UpdateManaVisuals();

        //choose between 5 cards
        CreateSelection();
    }

    void CreateSelection()
    {
        CardLoader.LoadCards(ref shownCards);
        GameObject cards = GameObject.Find("CardLoader");

        Vector3 centerPosition = Camera.main.transform.position + Camera.main.transform.forward * DESIGN.DESIGN_VALUES.CardSelectingDistanceToCamera;

        cards.transform.position = centerPosition;
        cards.transform.forward = Camera.main.transform.forward;

        int i = 0;

        foreach (Transform child in cards.transform)
        {
            //Set a new position
            child.transform.localPosition = new Vector3((1 * i) - (1 * (cards.transform.childCount - 3)), 0, 0);
            child.transform.Rotate(-90.0f, 0.0f, 0.0f);

            i++;
        }

        currentSpacesLeft = MapState.SpacesLeft();
    }

    //OnClick
    void PreSelectionCards(GameObject selection)
    {
        //If is on animation, don't do anything
        if (selection.CompareTag("AnimOn")) return;

        if (selectedCards.Contains(selection))
        {
            UnselectCard(selection);
        }
        else if (CanBeSelected(selection)) 
        { 
            SelectCard(selection);
        }
    }

    bool CanBeSelected(GameObject card)
    {
        //Check spaces
        if (currentSpacesLeft <= selectedCards.Count) return false;

        //Check mana
        if (card.GetComponent<CardValues>().manaCost > currentPlayerMana) return false;

        return true;
    }

    void SelectCard(GameObject card)
    {
        selectedCards.Add(card);

        currentPlayerMana -= card.GetComponent<CardValues>().manaCost;

        UpdateManaVisuals();

        //set a new color for the card (glowing)
        card.transform.Find("Canvas").Find("CardImage").GetComponent<Image>().color = DESIGN_VALUES.cardHighlightColor;

        StartCoroutine(Animation(card.transform, true));
    }

    void UnselectCard(GameObject card)
    {
        selectedCards.Remove(card);

        currentPlayerMana += card.GetComponent<CardValues>().manaCost;
        UpdateManaVisuals();

        //set the original color
        card.transform.Find("Canvas").Find("CardImage").GetComponent<Image>().color = Color.white;

        StartCoroutine(Animation(card.transform, false));
    }

    IEnumerator Animation(Transform card, bool up)
    {
        string ogTag = card.tag;
        card.tag = "AnimOn";

        Vector3 finalPos = card.localPosition;
        finalPos.y += DESIGN_VALUES.CardSelectingAnimationUpDistance * (up ? 1f : -1f);
        Vector3 velocity = Vector3.zero;
        while (card != null && Vector3.Distance(card.localPosition, finalPos) > 0.01f)
        {
            if (stopAnimating) break;
            card.localPosition = Vector3.SmoothDamp(card.localPosition, finalPos, ref velocity, DESIGN_VALUES.timeOnCardSelectingAnimation);

            yield return null;
        }
        if(card != null) card.tag = ogTag;
    }

    public void SendCardsToBoard()
    {
        stopAnimating = true;

        //Prepare cards to send to board (visually & removing from raycast layer)
        foreach (GameObject card in selectedCards)
        {
            card.transform.Find("Canvas").transform.Find("CardImage").GetComponent<Image>().color = Color.white;
            card.transform.Find("Canvas").transform.Find("ManaCost_Text").gameObject.SetActive(false);
            card.layer = 6;
        }

        //Setting player mana
        if (MapState.bottomPlayerAtacking)
        {
            MapState.bottomMana = currentPlayerMana;
        }
        else
        {
            MapState.topMana = currentPlayerMana;
        }
        currentPlayerMana = -1; //Reset mana value
        currentSpacesLeft = -1; //Reset spaces value

        //Send cards to board
        MapState.SetCardsOnMap(selectedCards);

        //Remove selected cards from the currentPlayerDeck
        foreach (GameObject card in selectedCards)
        {
            CardValues selectedValues = card.GetComponent<CardValues>();

            if(MapState.bottomPlayerAtacking)
            {
                bottomPlayerDeck.Remove(selectedValues.id);
            }
            else
            {
                topPlayerDeck.Remove(selectedValues.id);
            }
        }

        //Destroy non-selected cards
        Destroy(GameObject.Find("CardLoader"));

        //Clear the selected and drawn cards
        selectedCards.Clear();
        shownCards.Clear();
    }

    void UpdateManaVisuals()
    {
        panelSelectCards.transform.Find("Mana").Find("Mana_Text").GetComponent<TMP_Text>().text = currentPlayerMana.ToString();
    }

    void DetectCard()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast to detect tiles
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, cardLayer))
            {
                PreSelectionCards(hit.transform.gameObject);
            }
        }
    }
}
