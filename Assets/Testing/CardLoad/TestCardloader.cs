using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCardloader : MonoBehaviour
{
    void Start()
    {
        List<uint> l = new List<uint>() { 
        1, 3, 5, 6,
        };
        CardLoader.LoadCards(ref l);

        MapState.SpacesLeft();

        MapState.StartTurn();
        MapState.SpacesLeft();

        MapState.StartTurn();
    }
}
