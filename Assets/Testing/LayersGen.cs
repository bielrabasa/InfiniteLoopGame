using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LayersGen : MonoBehaviour
{
    public GameObject layerPrefab;
    public float numLayers;

    public bool player1;
    float maxDistance = 600;

    public List<GameObject> layerListP1;
    public List<GameObject> layerListP0;
    public List<GameObject> playerList;

    public Vector3 initialPos;
    public int distanceLayers;

    public bool layerYOrZ;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numLayers; i++)
        {
            GameObject newLayer = Instantiate(layerPrefab);
            newLayer.name = "Layer_" + i.ToString();
            newLayer.transform.SetParent(transform, false);

            //maxDistance = layerPrefab.GetComponent<RectTransform>().sizeDelta.y * numLayers;
            maxDistance = layerPrefab.transform.localScale.y * numLayers;

            float posY = (-maxDistance / 2) + (maxDistance * (i / (numLayers - 1)));

            if (layerYOrZ) newLayer.transform.localPosition = (new Vector3(0.0f, posY * distanceLayers, 0.0f) + initialPos);
            else newLayer.transform.localPosition = (new Vector3(0.0f, 0.0f, posY * distanceLayers) + initialPos);

            if (i == 0 || i == numLayers - 1) playerList.Add(newLayer);
            else if (i < numLayers/2)
            {
                //AddDrag(newLayer);
                layerListP0.Add(newLayer);
            }
            else
            {
                //AddDrag(newLayer);
                layerListP1.Add(newLayer);
            }
        }
    }

    void AddDrag(GameObject layer)
    {
        //Add the event PointerUp to select the card
        layer.AddComponent(typeof(EventTrigger));
        EventTrigger trigger = layer.GetComponent<EventTrigger>();

        //Not showed in the inspector, but is there
        EventTrigger.Entry drag = new EventTrigger.Entry();
        drag.eventID = EventTriggerType.Drag;
        drag.callback.AddListener(delegate { layer.GetComponent<MoveLayer>().DragLayer(); });
        trigger.triggers.Add(drag);
    }
}
