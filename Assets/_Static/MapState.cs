using System.Collections;
using System.Collections.Generic;
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

    //Physical board size
    static Vector2 boardSize = new Vector2(20, 30);
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
        //Debug.Log("Starting Turn for " + (bottomPlayerAtacking ? "BOTTOM" : "TOP") + " player.");

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

        //Switch Turns
        bottomPlayerAtacking = !bottomPlayerAtacking;
    }

    static IEnumerator CardAttack(int c, int r)
    {
        if (cardPositions[c, r] != null)
        {
            yield return cardPositions[c, r].GetComponent<CardValues>().Attack(new Vector2Int(c, r));
            yield return new WaitForSeconds(0.3f);
        }
    }
}
