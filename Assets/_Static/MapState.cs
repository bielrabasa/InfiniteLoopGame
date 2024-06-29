using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapState
{
    //---------------MAP STATE---------------
    public enum LayerPerks
    {
        NONE,
        MANA,
        DAMAGE,
        RANGE
    }

    //Layer positions (both players) [0 = up, 5 = down]
    public static LayerPerks[] Layer = new LayerPerks[6] 
    { LayerPerks.NONE, LayerPerks.NONE, LayerPerks.NONE, LayerPerks.NONE, LayerPerks.NONE, LayerPerks.NONE };

    //Card position on map (both players) [0 = up, 5 = down, 0 = left, 2 = right]
    public static GameObject[,] cardPositions = new GameObject[3, 6];

    static Transform cardHolder = null;
    static Vector2 canvasSize;

    static void CreateCardHolder()
    {
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>(); //TODO: Check if this is good?
        canvasSize = new Vector2(canvasRect.rect.width, canvasRect.rect.height); //TODO: Check if this is good?

        GameObject holder = new GameObject("CardHolder");
        cardHolder = holder.transform;

        //HARDCODED: (ROGER WTF why canvas does smth strange)
        cardHolder.parent = canvas.transform;
        cardHolder.localPosition = Vector3.zero;
        cardHolder.localScale = Vector3.one;
    }

    //---------------TURN SET CARDS---------------
    
    //Returns the number of spaces left
    public static int SpacesLeft()
    {
        int spaces = 0;

        for(int i = 0; i < 3; i++) //Horizontal
        {
            for (int j = 0; j < 3; j++) //Vertical
            {
                if (cardPositions[i, j + (bottomPlayerAtacking ? 3 : 0)] == null) spaces++;
            }
        }

        return spaces;
    }

    //Returns exactly the spaces left
    static void WhichSpacesLeft(ref List<Vector2Int> spaces)
    {
        for (int hori = 0; hori < 3; hori++) //Horizontal
        {
            for (int i = 0; i < 3; i++) //Vertical
            {
                int vert = i + (bottomPlayerAtacking ? 3 : 0);
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
        //TODO: Do this in another way, too complicated (we should have anchored places in the board
        // and a list of positions in which cards move)
        Vector3 pos = Vector3.zero;
        float horizontalStep = canvasSize.x / 4f;
        float verticalStep = canvasSize.y / 8f;
        pos.x = (-canvasSize.x / 2f) + horizontalStep / 2f + horizontalStep * gridPos.x;
        pos.y = canvasSize.y / 2f - verticalStep / 2f - verticalStep * gridPos.y - (gridPos.y >= 3 ? verticalStep : 0f);
        
        card.localPosition = pos;
        card.localScale = Vector3.one * 0.3f;
    }

    //---------------TURN ATTACK---------------
    public static bool bottomPlayerAtacking = true;

    public static void StartTurn()
    {
        if (bottomPlayerAtacking) //BOTTOM player
        {
            for(int l = 3; l <= 5; l++) // layers 3,4,5
            {
                for (int r = 0; r < 3; r++) //from left to right
                {
                    //Attack
                    //TODO: Call attack function
                    //Debug.Log("BOT = L:" + l + " R:" + r);
                }
            }
        }
        else //TOP player
        {
            for (int l = 2; l >= 0; l--) // layers 3,4,5
            {
                for (int r = 2; r >= 0; r--) //from right to left
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
