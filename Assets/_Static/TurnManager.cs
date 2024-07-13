using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public void DrawingCardsAcceptButton()
    {
        StartCoroutine(MapState.NextPhase());
    }

    public void NextButton()
    {
        if(MapState.turnPhase == MapState.TurnPhase.LAYER_MOVING)
            StartCoroutine(MapState.NextPhase());
    }
}
