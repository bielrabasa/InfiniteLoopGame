using DESIGN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardZoom : MonoBehaviour
{
    [SerializeField] LayerMask cardLayer;

    Transform card;
    Vector3 ogPos;

    void Update()
    {
        if (MapState.turnPhase != MapState.TurnPhase.LAYER_MOVING) return;
        DetectCard();
    }

    void DetectCard()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (card != null)
        {
            card.position = ogPos;
            card = null;
        }

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, cardLayer))
        {
            card = hit.transform;
            ogPos = card.position;
            card.position = Camera.main.transform.position + Camera.main.transform.forward * DESIGN_VALUES.CardZoomDistanceToCamera;
        }

        // Raycast to detect tiles
        /*if (!Physics.Raycast(ray, out hit, Mathf.Infinity, cardLayer)) {
            //If card was selected, bring to originalPosition
            if(card != null)
            {
                card.transform.localPosition = ogPos;
                card = null;
            }
            return;
        }

        card = hit.transform;
        Vector3 frontCamera = Camera.main.transform.position + Camera.main.transform.forward * 2f;
        
        //Move it to front
        if(card.position != frontCamera)
        {
            ogPos = card.position;
            card.position = frontCamera;
        }
        else if(card != null)
        {
            card.transform.localPosition = ogPos;
            card = null;
        }*/
    }
}
