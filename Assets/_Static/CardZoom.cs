using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardZoom : MonoBehaviour
{
    [SerializeField] LayerMask cardLayer;

    GameObject card;
    Vector3 ogPos;
    Vector3 frontCamera;

    // Start is called before the first frame update
    void Start()
    {
        frontCamera = FindObjectOfType<Camera>().transform.position;
        Debug.Log(frontCamera);

        frontCamera.x += 0.44f;
        frontCamera.y -= 5.62f;
        frontCamera.z += 1.14f;
        //-2.19 29.15 -6.39
        //-0.44  +5.62 -1.14
        //-1.75f, 23.5f, -5.25f
        Debug.Log(frontCamera);
    }

    // Update is called once per frame
    void Update()
    {
        DetectCard();
    }

    void DetectCard()
    {
        if (MapState.turnPhase == MapState.TurnPhase.LAYER_MOVING)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast to detect tiles
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, cardLayer))
            {
                card = hit.transform.gameObject;

                if(card.transform.localPosition != frontCamera)
                    ogPos = card.transform.localPosition;

                //set in front the camera
                card.transform.localPosition = frontCamera;
            }
            else if(card != null)
            {
                //set in front the camera
                card.transform.localPosition = ogPos;
                card = null;
            }
        }
        else
        {
            if (card != null)
            {
                //set in front the camera
                card.transform.localPosition = ogPos;
                card = null;
            }
        }
    }
}
