using DESIGN;
using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float initPos;

    void Start()
    {
        initPos = transform.position.z;
    }

    public IEnumerator SwitchPosition()
    {
        Vector3 pos = transform.position;
        float pini = transform.position.z;
        float pend = -pini;

        Quaternion r = transform.rotation;
        Quaternion rend = Quaternion.Euler(transform.rotation.eulerAngles + (Vector3.up * 180f));

        float time = 0f;
        while(time < 1f)
        {
            pos.z = Mathf.Lerp(pini, pend, time);
            transform.rotation = Quaternion.Slerp(r, rend, time);
            transform.position = pos;

            time += Time.deltaTime / DESIGN_VALUES.timeOnCameraSwitching;
            yield return null;
        }

        pos.z = pend;
        transform.position = pos;
        transform.rotation = rend;
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
