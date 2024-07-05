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

    // Start is called before the first frame update
    void Start()
    {
        thisObject = transform;
        //basisObject = transform.parent.GetComponent<RectTransform>();
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
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isClick = false;
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
}
