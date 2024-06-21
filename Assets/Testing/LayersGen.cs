using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LayersGen : MonoBehaviour
{
    public GameObject layerPrefab;
    public float numLayers;

    float maxDistance = 600;

    public List<GameObject> layerList;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject parentLayer = GameObject.Find("LayersParent");

        for (int i = 0; i < numLayers; i++)
        {
            GameObject newLayer = Instantiate(layerPrefab);
            newLayer.name = "Layer_" + i.ToString();
            newLayer.transform.SetParent(transform, false);

            maxDistance = layerPrefab.GetComponent<RectTransform>().sizeDelta.y * numLayers + 25f;

            float posY = (-maxDistance / 2) + (maxDistance * (i / (numLayers - 1)));

            newLayer.transform.localPosition = new Vector3(0.0f, posY, 0.0f);

            layerList.Add(newLayer);
        }
    }
}
