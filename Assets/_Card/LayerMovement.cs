using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerMovement : MonoBehaviour
{
    const string PHYSICAL_LAYER = "3DLayers"; //3D_Layer
    public GameObject LayerPrefab;

    public Material rangeMat;
    public Material damageMat;
    public Material manaMat;

    Transform[] layers = new Transform[MapState.ROWS];
    float[] pos;

    bool isDragging;
    Transform draggingLayer;
    int draggingLayerIndex;

    void Start()
    {
        pos = MapState.GetVerticalBoardPositions();
        for(int i = 0; i < MapState.ROWS; i++)
        {
            //GameObject go = Instantiate(LayerPrefab, new Vector3(MapState.boardSize.x / 2f + 5f, -6.4f, pos[i]), Quaternion.identity, transform);
            GameObject go = Instantiate(LayerPrefab, new Vector3(0, -6.4f, pos[i]), Quaternion.identity, transform);
            go.transform.localScale = new Vector3(1.0f, 1.0f, 0.5f);
            go.layer = 7;
            layers[i] = go.transform;

            //Initialize perks
            switch(i % 3)
            {
                case 0:
                    go.name = "Range";
                    go.GetComponentInChildren<Renderer>().material = rangeMat;
                    break;
                case 1:
                    go.name = "Damage";
                    go.GetComponentInChildren<Renderer>().material = damageMat;
                    break;
                case 2:
                    go.name = "Mana";
                    go.GetComponentInChildren<Renderer>().material = manaMat;
                    break;
            }
        }
    }

    void Update()
    {
        if (MapState.turnPhase != MapState.TurnPhase.LAYER_MOVING) return;

        //Pick layer
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast to detect tiles
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask(PHYSICAL_LAYER)))
            {
                if ((MapState.bottomPlayerAtacking && hit.transform.position.z < 0) ||
                    !MapState.bottomPlayerAtacking && hit.transform.position.z > 0)
                {
                    isDragging = true;
                    draggingLayer = hit.transform;
                    draggingLayerIndex = Array.IndexOf(layers, draggingLayer);
                }
            }
        }

        //Drag layer
        if(isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Calculate the distance from the camera to the Y=0 plane
            float distanceToPlane = -ray.origin.y / ray.direction.y;

            // Get the world position of the mouse with Y set to 0
            Vector3 mouseWorldPosition = ray.GetPoint(distanceToPlane);

            Vector3 p = draggingLayer.position;
            p.z = mouseWorldPosition.z;

            if(MapState.bottomPlayerAtacking)
            {
                p.z = Mathf.Clamp(p.z, pos[MapState.ROWS - 1], pos[MapState.ROWS / 2]);
            }
            else
            {
                p.z = Mathf.Clamp(p.z, pos[(MapState.ROWS / 2) - 1], pos[0]);
            }

            draggingLayer.position = p;

            //Layer Swap
            for (int i = 0; i < pos.Length; i++)
            {
                if (i == draggingLayerIndex) continue;

                if (Mathf.Abs(pos[i] - p.z) < 1f)
                {
                    //Swap
                    layers[draggingLayerIndex] = layers[i];
                    layers[i] = draggingLayer;

                    Vector3 swapPos = layers[draggingLayerIndex].position;
                    swapPos.z = pos[draggingLayerIndex];
                    layers[draggingLayerIndex].position = swapPos;

                    draggingLayerIndex = i;
                }
            }
        }

        //Drop layer
        if (Input.GetMouseButtonUp(0) && draggingLayer != null)
        {
            Vector3 snapPos = draggingLayer.position;
            snapPos.z = pos[draggingLayerIndex];
            draggingLayer.position = snapPos;

            isDragging = false;
            draggingLayer = null;
        }
    }

    public void TransferInformation()
    {
        for(int i = 0; i < layers.Length; i++)
        {
            switch (layers[i].name)
            {
                case "Range":
                    MapState.Layer[i] = MapState.LayerPerks.RANGE; 
                    break;
                case "Damage":
                    MapState.Layer[i] = MapState.LayerPerks.DAMAGE;
                    break;
                case "Mana":
                    MapState.Layer[i] = MapState.LayerPerks.MANA;
                    break;
            }
        }
    }
}
