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
    public static GameObject[,] cardPositions = new GameObject[6, 3];

    static Transform cardHolder = null;

    static void CreateCardHolder()
    {
        GameObject holder = new GameObject("CardHolder");
        cardHolder = holder.transform;
    }

    //---------------TURN SET CARDS---------------
    
    //Returns the number of spaces left
    public static int SpacesLeft()
    {
        int spaces = 0;

        for(int i = 0; i < 3; i++) //Vertical
        {
            for (int j = 0; j < 3; j++) //Horizontal
            {
                if (cardPositions[i + (bottomPlayerAtacking ? 3 : 0), j] == null) spaces++;
            }
        }

        return spaces;
    }

    //Returns exactly the spaces left
    static void WhichSpacesLeft(ref List<Vector2Int> spaces)
    {
        for (int i = 0; i < 3; i++) //Vertical
        {
            for (int hori = 0; hori < 3; hori++) //Horizontal
            {
                int vert = i + (bottomPlayerAtacking ? 3 : 0);
                if (cardPositions[vert, hori] == null) spaces.Add(new Vector2Int(vert, hori));
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
            //TODO: Get random number from spaces list
            //TODO: erase number from spaces list
            //TODO: Set card on cardPositions array
        }
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
                    //Debug.Log("TOP = L:" + l + " R:" + r);
                }
            }
        }

        //Switch Turns
        bottomPlayerAtacking = !bottomPlayerAtacking;
    }
}
