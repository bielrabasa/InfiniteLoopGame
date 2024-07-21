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

    public void StartGame()
    {
        StartCoroutine(MapState.NextPhase());
    }
    IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(1);
        StartGame();
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
        SceneManager.LoadScene("MainScene");
    }
}
