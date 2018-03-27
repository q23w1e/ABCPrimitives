using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomGradient
{
    public ColorKeys ColorKeys = new ColorKeys();

    public CustomGradient()
    {
        ColorKeys.Add(Color.black, 0f);
        ColorKeys.Add(Color.white, 1f);
    }

    public Color Evaluate(float time)
    {
        int ti = 0;
        for (int i = 0; i < ColorKeys.Count - 1; i++)
        {
            if ((time >= ColorKeys[i].Time) && (time <= ColorKeys[i + 1].Time))  
            {
                ti = i;
                break;
            }
        }
        // InverseLerp: (t - ti) / (tii - ti)
        float localTime = Mathf.InverseLerp(ColorKeys[ti].Time, ColorKeys[ti + 1].Time, time);
        
        return Color.Lerp(ColorKeys[ti].Color, ColorKeys[ti + 1].Color, localTime);
    }

    public Texture2D GetTexture(int width)
    {
        Texture2D texture = new Texture2D(width, 1);
        Color[] pixelColors = new Color[width];
        
        for (int i = 0; i < pixelColors.Length; i++)
        {
            float t = (float)i / (width - 1);
            pixelColors[i] = Evaluate(t);
        }
        
        texture.SetPixels(pixelColors);
        texture.Apply();

        return texture;
    }
}
