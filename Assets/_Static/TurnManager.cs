using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public void DrawingCardsAcceptButton()
    {
        AudioManager.SetSFX(AudioManager.SFX.BUTTON);

        if (MapState.turnPhase == MapState.TurnPhase.CARD_SELECTING)
            StartCoroutine(MapState.NextPhase());
    }

    public void NextButton()
    {
        AudioManager.SetSFX(AudioManager.SFX.BUTTON);

        if (MapState.turnPhase == MapState.TurnPhase.LAYER_MOVING)
            StartCoroutine(MapState.NextPhase());
    }
}
