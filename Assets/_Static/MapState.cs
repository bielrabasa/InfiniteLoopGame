using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public static class MapState
{
    const int COLUMNS = 3;
    const int ROWS = 6;

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

    //Physical board size
    static Vector2 boardSize = new Vector2(20, 30);
    static Vector3[,] boardPositions = null;

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

    static void SetCardOnPhysicalBoard(Transform card, Vector2Int gridPos)
    {
        if(boardPositions == null) CreateBoardPositions();

        //TODO: This will be made into a Lerp function
        card.localPosition = boardPositions[gridPos.x, gridPos.y];
    }

    //---------------TURN ATTACK---------------
    public static bool bottomPlayerAtacking = true;

    public static void StartTurn()
    {
        if (bottomPlayerAtacking) //BOTTOM player
        {
            for(int l = ROWS/2; l < ROWS; l++) // layers 3,4,5
            {
                for (int r = 0; r < COLUMNS; r++) //from left to right
                {
                    //Attack
                    //TODO: Call attack function
                    //Debug.Log("BOT = L:" + l + " R:" + r);
                }
            }
        }
        else //TOP player
        {
            for (int l = ROWS / 2 - 1; l >= 0; l--) // layers 3,4,5
            {
                for (int r = COLUMNS - 1; r >= 0; r--) //from right to left
                {
                    //Attack
                    //TODO: Call attack function
                    //Debug.Log("TOP = L:" + l + " R:" + r);
                }
            }
        }

        //Switch Turns
        bottomPlayerAtacking = !bottomPlayerAtacking;
    }
}
