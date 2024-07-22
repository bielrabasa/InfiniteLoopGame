using DESIGN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardZoom : MonoBehaviour
{
    [SerializeField] LayerMask cardLayer;

    const float zoomScale = 7f;

    Transform card;
    Vector3 ogPos;

    bool sound = true;

    void Update()
    {
        if (MapState.turnPhase != MapState.TurnPhase.LAYER_MOVING) return;
        DetectCard();
    }

    void DetectCard()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, cardLayer))
        {
            if (card == hit.transform) return;
            if(card != null) ReturnCardToPos(); //There was other card zoomed

            //Make zoom
            card = hit.transform;
            ogPos = card.position;
            card.position = Camera.main.transform.position + Camera.main.transform.forward * DESIGN_VALUES.CardZoomDistanceToCamera
                + (card.forward * 5f);
            
            Vector3 s = card.localScale;
            s *= zoomScale;
            card.localScale = new Vector3(s.x, card.localScale.y, s.z);

            if (sound)
            {
                AudioManager.SetSFX(AudioManager.SFX.SELECTCARD);
                sound = false;
            }
        }
        else
        {
            if (card != null) ReturnCardToPos();

            if (!sound)
            {
                AudioManager.SetSFX(AudioManager.SFX.DESSELECTCARD);
                sound = true;
            }
        }
    }

    void ReturnCardToPos()
    {
        card.position = ogPos;

        Vector3 s = card.localScale;
        s /= zoomScale;
        card.localScale = new Vector3(s.x, card.localScale.y, s.z);
        card = null;
    }
}

