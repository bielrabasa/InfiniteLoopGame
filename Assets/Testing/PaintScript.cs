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

        //Test
        Paint(0, 0, Color.green, 10);
        Paint(10, 10, Color.red, 15);
        Paint(5, 5, Color.blue, 20);
    }

    void Paint(int x, int y, Color color, int size)
    {
        int centerX = (textureSize / 2 - x);
        int centerY = (textureSize / 2 - y);
        int rad = size / 2;

        for (int i = centerX - rad; i < centerX + rad; i++)
        {
            for(int j = centerY - rad; j < centerY + rad; j++)
            {
                if ((i - centerX) * (i - centerX) + (j - centerY) * (j - centerY) < size)
                    colors[i * textureSize + j] = color;
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
    void Update()
    {
        
    }
}
