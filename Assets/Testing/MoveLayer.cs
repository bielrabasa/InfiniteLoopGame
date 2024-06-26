using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLayer : MonoBehaviour
{
    RectTransform thisObject;
    RectTransform basisObject;

    // Start is called before the first frame update
    void Start()
    {
        thisObject = this.GetComponent<RectTransform>();
        basisObject = transform.parent.GetComponent<RectTransform>();
    }

    public void DragLayer()
    {
        Vector3 offset = new Vector3(863, 0, 93);
        Vector3 screenPosition = Input.mousePosition + offset;

        screenPosition.x = basisObject.position.x;
        screenPosition.z = basisObject.position.z;

        thisObject.position = Camera.main.ScreenToWorldPoint(screenPosition);

        thisObject.localPosition = new Vector3(0, thisObject.position.y, 0);
    }
}
