using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{


    void Start()
    {
        
    }

    public void MoveToPosition(Vector3 pos)
    {
        StartCoroutine(MoveToPos(pos));
    }

    IEnumerator MoveToPos(Vector3 pos)
    {
        Vector3 velocity = Vector3.zero;
        while (Vector3.Distance(transform.position, pos) > 0.01f)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, pos, ref velocity, 1f);
            yield return null;
        }
    }
}
