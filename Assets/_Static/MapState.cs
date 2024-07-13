using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public static class MapState
{
    public const int COLUMNS = 3;
    public const int ROWS = 6;

    //---------------MAP STATE---------------
    public enum LayerPerks
    {
        NONE,
        MANA,
        DAMAGE,
        RANGE
    }

    //Layer positions (both players) [0 = up, 5 = down]
    public static LayerPerks[] Layer = new LayerPerks[ROWS] 
    { LayerPerks.NONE, LayerPerks.NONE, LayerPerks.NONE, LayerPerks.NONE, LayerPerks.NONE, LayerPerks.NONE };

    //Card position on map (both players) [0 = left, 2 = right, 0 = up, 5 = down]
    public static GameObject[,] cardPositions = new GameObject[COLUMNS, ROWS];

    static Transform cardHolder = null;

    //Player information
    public static bool bottomPlayerAtacking = true;
    public static int BottomHeroHP = 30; //TODO: Make it not hardcoded
    public static int TopHeroHP = 30;
    public static int bottomMana = 5; //Starting mana value
    public static int topMana = 7;
    const int MAX_MANA = 10;

    public enum TurnPhase
    {
        NONE,
        CARD_SELECTING,
        LAYER_MOVING,
        ATTACKING
    }
    public static TurnPhase turnPhase = TurnPhase.NONE;

    //Physical board size
    public static Vector2 boardSize = new Vector2(20, 30);
    public static Vector3[,] boardPositions = null;
    public static Vector3 bottomHeroPosition;
    public static Vector3 topHeroPosition;

    static void CreateCardHolder()
    {
        GameObject holder = new GameObject("CardHolder");
        cardHolder = holder.transform;
    }
    static void CreateBoardPositions()
    {
        boardPositions = new Vector3[COLUMNS, ROWS];

        for (int i = 0; i < COLUMNS; i++)
        {
            for (int j = 0; j < ROWS; j++)
            {
                Vector3 pos = Vector3.zero;
                float horizontalStep = boardSize.x / COLUMNS;
                float verticalStep = boardSize.y / ROWS;

                pos.x = horizontalStep / 2f + (-boardSize.x / 2f) + horizontalStep * i;
                pos.y = 1;
                pos.z = - verticalStep / 2f + (boardSize.y / 2f) - verticalStep * j;

                boardPositions[i, j] = pos;
            }
        }

        bottomHeroPosition = new Vector3(0, 0, -boardSize.y / 2f);
        topHeroPosition = new Vector3(0, 0, boardSize.y / 2f);
    }

    public static float[] GetVerticalBoardPositions()
    {
        if (boardPositions == null) CreateBoardPositions();

        float[] vPos = new float[ROWS];
        for (int i = 0; i < ROWS; i++) vPos[i] = boardPositions[0, i].z;

        return vPos;
    }

    //---------------TURN SET CARDS---------------
    
    //Returns the number of spaces left
    public static int SpacesLeft()
    {
        int spaces = 0;

        for(int i = 0; i < COLUMNS; i++)
        {
            for (int j = 0; j < ROWS / 2; j++)
            {
                if (cardPositions[i, j + (bottomPlayerAtacking ? ROWS / 2 : 0)] == null) spaces++;
            }
        }

        return spaces;
    }

    //Returns exactly the spaces left
    static void WhichSpacesLeft(ref List<Vector2Int> spaces)
    {
        for (int hori = 0; hori < COLUMNS; hori++) //Horizontal
        {
            for (int i = 0; i < ROWS / 2; i++) //Vertical
            {
                int vert = i + (bottomPlayerAtacking ? ROWS / 2 : 0);
                if (cardPositions[hori, vert] == null) spaces.Add(new Vector2Int(hori, vert));
            }
        }
    }

    public static void SetCardsOnMap(List<GameObject> cards)
    {
        if (cardHolder == null) CreateCardHolder();

        //Change cards parent
        foreach (GameObject card in cards) card.transform.SetParent(cardHolder);

        //Set selected cards randomly
        List<Vector2Int> spaces = new List<Vector2Int>();
        WhichSpacesLeft(ref spaces);
        foreach(GameObject card in cards)
        {
            if(spaces.Count == 0)
            {
                Debug.Log("[MapState] Selected cards exceed board space, not placing card.");
                continue;
            }

            //Get Random from empty spaces & erase from posible positions
            Vector2Int cardPos = spaces[Random.Range(0, spaces.Count)];
            spaces.Remove(cardPos);

            //Set Card to array & set in physical board
            cardPositions[cardPos.x, cardPos.y] = card;
            SetCardOnPhysicalBoard(card.transform, cardPos);
        }
    }

    public static void SetCardOnPhysicalBoard(Transform card, Vector2Int gridPos)
    {
        if(boardPositions == null) CreateBoardPositions();

        card.GetComponent<Ability>().myStartPosition = gridPos;

        //TODO: This will be made into a Lerp function
        card.localPosition = boardPositions[gridPos.x, gridPos.y];

        //TODO: Hardcoded scale
        card.localScale *= 4f;
    }

    //---------------TURN ATTACK---------------

    public static IEnumerator StartTurn()
    {
        IterateStartEndTurn(bottomPlayerAtacking, true); //Iterate the player attacking
        IterateStartEndTurn(!bottomPlayerAtacking, true); //Iterate the player defending

        //Attack
        if (bottomPlayerAtacking) //BOTTOM player
        {
            for(int r = ROWS / 2; r < ROWS; r++) // layers 3,4,5
            {
                for (int c = 0; c < COLUMNS; c++) //from left to right
                {
                    yield return CardAttack(c, r);
                }
            }
        }
        else //TOP player
        {
            for (int r = ROWS / 2 - 1; r >= 0; r--) // layers 2,1,0
            {
                for (int c = COLUMNS - 1; c >= 0; c--) //from right to left
                {
                    yield return CardAttack(c, r);
                }
            }
        }

        IterateStartEndTurn(bottomPlayerAtacking, false); //Iterate the player attacking
        IterateStartEndTurn(!bottomPlayerAtacking, false); //Iterate the player defending

        //Switch Turns
        bottomPlayerAtacking = !bottomPlayerAtacking;
        Debug.Log("Switching turns!");
    }

    static IEnumerator CardAttack(int c, int r)
    {
        if (cardPositions[c, r] != null)
        {
            yield return cardPositions[c, r].GetComponent<CardValues>().Attack(new Vector2Int(c, r));
            yield return new WaitForSeconds(0.3f);
        }
    }

    //----------------------------------

    static void IterateStartEndTurn(bool bottomPlayer, bool start)
    {
        //Iterate the bottom player cards
        if (bottomPlayer)
        {
            for (int r = ROWS / 2; r < ROWS; r++) // layers 3,4,5
                for (int c = 0; c < COLUMNS; c++) //from left to right
                    InitEndTurn(c, r, start, (bottomPlayer == bottomPlayerAtacking));
        }
        else
        {
            for (int r = ROWS / 2 - 1; r >= 0; r--) // layers 2,1,0
                for (int c = COLUMNS - 1; c >= 0; c--) //from right to left
                    InitEndTurn(c, r, start, (bottomPlayer == bottomPlayerAtacking));
        }
    }

    static void InitEndTurn(int c, int r, bool start, bool isPlayer)
    {
        if (cardPositions[c, r] != null)
        {
            //Debug.Log((start ? "Starting " : "Ending ") + (r <= 2? "Top ": "Bottom ") + (isPlayer ? "Player " : "Enemy ") + "Turn");

            if (start)
            {
                //START turn
                if (isPlayer)
                    cardPositions[c, r].GetComponent<CardValues>().abilityScript.OnStartPlayerTurn();
                else
                    cardPositions[c, r].GetComponent<CardValues>().abilityScript.OnStartEnemyTurn();
            }
            else
            {
                //END turn
                if(isPlayer)
                    cardPositions[c, r].GetComponent<CardValues>().abilityScript.OnEndPlayerTurn();
                else
                    cardPositions[c, r].GetComponent<CardValues>().abilityScript.OnEndEnemyTurn();
            }
        }
    }

    //---------------GAME LOOP---------------

    public static IEnumerator NextPhase()
    {
        switch(turnPhase)
        {
            case TurnPhase.NONE:
                Debug.Log("CardSelecting! " + (bottomPlayerAtacking ? "BOT" : "TOP"));
                turnPhase = TurnPhase.CARD_SELECTING;
                GameObject.Find(bottomPlayerAtacking ? "DeckP0" : "DeckP1").
                    GetComponent<CardsOnLayers>().DrawCards();

                break;
            case TurnPhase.CARD_SELECTING:
                GameObject.Find(bottomPlayerAtacking ? "DeckP0" : "DeckP1").
                    GetComponent<CardsOnLayers>().CreateCards();
                Debug.Log("LayerMoving! " + (bottomPlayerAtacking? "BOT" : "TOP"));
                turnPhase = TurnPhase.LAYER_MOVING;

                break;
            case TurnPhase.LAYER_MOVING: 
                Debug.Log("Attacking! " + (bottomPlayerAtacking ? "BOT" : "TOP"));
                turnPhase = TurnPhase.ATTACKING;
                GameObject.FindObjectOfType<LayerMovement>().TransferInformation();
                yield return StartTurn();

                break;
            case TurnPhase.ATTACKING:
                Debug.Log("CardSelecting! " + (bottomPlayerAtacking ? "BOT" : "TOP"));
                turnPhase = TurnPhase.CARD_SELECTING;
                GameObject.Find(bottomPlayerAtacking ? "DeckP0" : "DeckP1").
                    GetComponent<CardsOnLayers>().DrawCards();

                break;
        }
    }
}
