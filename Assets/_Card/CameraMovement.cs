using DESIGN;
using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public IEnumerator SwitchPosition()
    {
        Transform botHero = GameObject.Find("BotHero").transform.GetChild(0).transform;
        Transform topHero = GameObject.Find("TopHero").transform.GetChild(0).transform;

        Quaternion rend = Quaternion.Euler(transform.rotation.eulerAngles + (Vector3.up * 180f));

        float time = 0f;
        while(time < 1f)
        {
            float rotAngle = Time.deltaTime / DESIGN_VALUES.timeOnCameraSwitching * 180f;

            transform.RotateAround(Vector3.zero, Vector3.up, rotAngle);
            botHero.RotateAround(botHero.parent.position, Vector3.up, rotAngle);
            topHero.RotateAround(topHero.parent.position, Vector3.up, rotAngle);

            time += Time.deltaTime / DESIGN_VALUES.timeOnCameraSwitching;
            yield return null;
        }

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
