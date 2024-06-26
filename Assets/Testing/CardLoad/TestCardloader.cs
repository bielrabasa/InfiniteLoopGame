using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCardloader : MonoBehaviour
{
    void Start()
    {
        List<int> l = new List<int>() { 
        1, 3, 5, 6
        };
        CardLoader.LoadCards(ref l);
    }
}
