using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float initPos;

    void Start()
    {
        initPos = transform.position.z;
    }

    public void SwitchPosition()
    {
        Vector3 p = transform.position;
        p.z = -p.z;
        transform.position = p;

        Vector3 r = transform.rotation.eulerAngles;
        if (r.y == 0.0f) r.y = 180f;
        else r.y = 0.0f;
        transform.rotation = Quaternion.Euler(r);
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
