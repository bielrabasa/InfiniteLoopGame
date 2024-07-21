using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(0.3f);
        yield return MapState.NextPhase();
    }

    void Start()
    {
        StartCoroutine(WaitForStart());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    public void ResetGame()
    {
        StartCoroutine(Rst());
    }

    IEnumerator Rst()
    {
        yield return new WaitForSeconds(5f);
        MapState.gameEnded = false;

        SceneManager.LoadScene("MainScene");
        
    }
}
