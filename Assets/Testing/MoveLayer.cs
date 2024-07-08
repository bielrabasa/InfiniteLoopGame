using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLayer : MonoBehaviour
{
    Transform thisObject;
    //RectTransform basisObject;

    //public float mouseSensitivity;

    public LayerMask layers;

    bool isClick = false;

    LayersGen listLayers;

    Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        thisObject = transform;
        //basisObject = transform.parent.GetComponent<RectTransform>();
        listLayers = transform.parent.GetComponent<LayersGen>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast to detect tiles
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layers))
            {
                if(hit.transform.gameObject == this.gameObject)
                {
                    isClick = true;
                    initialPos = transform.position;
                    Debug.Log(initialPos);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isClick = false;
            AttachLayer();
        }

        if (isClick) DragLayer();
    }

    public void DragLayer()
    {
        Vector3 screenPosition = Input.mousePosition;

        //screenPosition.x = basisObject.position.x;
        //screenPosition.z = basisObject.position.z;

        //thisObject.position = Camera.main.ScreenToWorldPoint(screenPosition);
        thisObject.position = (screenPosition * 0.01f) - new Vector3(0, 4.5f, 0);

        thisObject.localPosition = new Vector3(0, thisObject.position.y, 0);
        //thisObject.localPosition *= mouseSensitivity;
    }

    public void AttachLayer()
    {
        GameObject aboveLayer = null;
        GameObject belowLayer = null;

        if (initialPos.y < 0)
        {
            foreach (GameObject layers in listLayers.layerListP0)
            {
                /*if (transform.position.y < layers.transform.position.y) aboveLayer = layers;
                else belowLayer = layers;*/
            }
        }
        else
        {
            foreach (GameObject layers in listLayers.layerListP1)
            {

            }
            //Debug.Log(listLayers.layerListP1[0].name);
        }

        float distanceA = 0;
        float distanceB = 0;

        if(aboveLayer != null)
        distanceA = Mathf.Abs(transform.position.y - aboveLayer.transform.position.y);
        if(belowLayer != null)
        distanceB = Mathf.Abs(transform.position.y - belowLayer.transform.position.y);

        if(distanceA > distanceB) { }
        else { }
    }
}
