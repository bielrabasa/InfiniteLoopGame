using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndivParticleScript : MonoBehaviour
{
    const float dieTime = 1f;
    TMP_Text textObject;

    public void Init(string text, Color color)
    {
        textObject = transform.Find("Canvas").Find("ValueText").GetComponent<TMP_Text>();

        textObject.text = text;
        textObject.color = color;

        //Particles rotation to camera
        if (!MapState.bottomPlayerAtacking) transform.eulerAngles = new Vector3(90, 180, 0);
    }

    void Start()
    {
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        Destroy(gameObject, dieTime);

        while (true)
        {
            Color a = textObject.color;
            a.a -= (1f / dieTime) * Time.deltaTime;
            textObject.color = a;
            yield return null;
        }
    }
}
