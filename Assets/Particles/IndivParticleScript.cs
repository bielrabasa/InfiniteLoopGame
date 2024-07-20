using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndivParticleScript : MonoBehaviour
{
    public void Init(string text, Color color)
    {
        TMP_Text textObject = transform.Find("Canvas").Find("ValueText").GetComponent<TMP_Text>();

        textObject.text = text;
        textObject.color = color;
    }

    void Start()
    {
        Destroy(gameObject, 1);
    }
}
