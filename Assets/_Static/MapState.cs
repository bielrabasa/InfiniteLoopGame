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
    
    public static void SetCardsOnMap(List<GameObject> cards)
    {
        if (cardHolder == null) CreateCardHolder();

        foreach (GameObject card in cards) 
        {
            card.transform.SetParent(cardHolder);
        }

        //TODO: Set randomly, check for space
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
