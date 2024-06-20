using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LayersGen : MonoBehaviour
{
    public GameObject LayerPrefab;
    public int numLayers;

    public float maxDistance;

    // Start is called before the first frame update
    void Start()
    {
        GameObject parentLayer = GameObject.Find("LayersParent");

        for (int i = 0; i < numLayers; i++)
        {
            GameObject newLayer = Instantiate(LayerPrefab);
            newLayer.name = "Layer_" + i.ToString();
            newLayer.transform.SetParent(parentLayer.transform, false);

            float posY = ((maxDistance / numLayers) * (i + 1)) - (maxDistance);

            newLayer.transform.localPosition = new Vector3(0.0f, posY, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
