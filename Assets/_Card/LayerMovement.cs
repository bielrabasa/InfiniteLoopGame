using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerMovement : MonoBehaviour
{
    const string PHYSICAL_LAYER = "3DLayers"; //3D_Layer
    public GameObject LayerPrefab; 

    Transform[] layers = new Transform[MapState.ROWS];

    bool isDragging;
    Transform draggingLayer;


    void Start()
    {
        float[] pos = MapState.GetVerticalBoardPositions();
        for(int i = 0; i < MapState.ROWS; i++)
        {
            GameObject go = Instantiate(LayerPrefab, new Vector3(MapState.boardSize.x / 2f + 5f, 0, pos[i]), Quaternion.identity, transform);
            go.layer = 7;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Pick layer
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast to detect tiles
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask(PHYSICAL_LAYER)))
            {
                isDragging = true;
                draggingLayer = hit.transform;
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
            draggingLayer.position = p;
        }

        //Drop layer
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            draggingLayer = null;
        }
    }
}
