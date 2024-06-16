using System;
using UnityEngine;

public class PaintScript : MonoBehaviour
{
    public int textureSize = 2;

    Texture2D tex;
    Renderer r;

    Color32[] colors;

    void Start()
    {
        r = GetComponent<Renderer>();

        tex = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);
        tex.wrapMode = TextureWrapMode.Mirror;

        colors = new Color32[textureSize * textureSize];
        for (int i = 0; i < colors.Length; i++) colors[i] = Color.white;
    }

    void Paint(Vector2 p, Color color, int size)
    {
        int centerX = (textureSize / 2 - (int)p.x);
        int centerY = (textureSize / 2 - (int)p.y);

        int actionArea = size; //Area to check for color change

        for (int i = centerX - actionArea; i < centerX + actionArea; i++)
        {
            for(int j = centerY - actionArea; j < centerY + actionArea; j++)
            {
                if (i < 0 || j < 0 || i >= textureSize || j >= textureSize) continue;

                float dist = (i - centerX) * (i - centerX) + (j - centerY) * (j - centerY);
                if (dist < size) colors[i * textureSize + j] = Color.Lerp(color, colors[i * textureSize + j], dist / size);
            }
        }

        UpdatePaint();
    }

    void UpdatePaint()
    {
        //Set color array to pixels
        tex.SetPixels32(colors);

        //Apply
        tex.Apply();
        r.material.mainTexture = tex;
    }

    //---------------MOUSE-CLICK-----------
    private void Update()
    {
        if (Input.GetMouseButton(0)) CheckDrawClick();
    }

    void CheckDrawClick()
    {
        // Create a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                Vector3 clickPosition = hit.point;
                Vector2 p = PlaneToTextureCoords(WorldToPlaneCoords(clickPosition));
                Paint(p, Color.magenta, 10);
            }
        }
    }

    Vector2 WorldToPlaneCoords(Vector3 hitPoint)
    {
        Vector3 planeHit = hitPoint - transform.position;

        Vector2 pos = new Vector2(planeHit.x, planeHit.z);
        pos /= 5f * transform.localScale.x; //Set plane coords between -1 & 1 (scale should be the same in X & Z);

        return pos;
    }

    Vector2 PlaneToTextureCoords(Vector2 p)
    {
        Vector2 res = p * textureSize / 2;
        return new Vector2(res.y, res.x); //Texture Coords are inverted
    }
}
