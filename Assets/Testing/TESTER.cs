using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTER : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MapState.SetCardsOnMap(new List<GameObject>() { GameObject.CreatePrimitive(PrimitiveType.Cube) });
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            MapState.bottomPlayerAtacking = !MapState.bottomPlayerAtacking;
        }
    }
}
