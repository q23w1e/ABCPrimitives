using System.Collections.Generic;
using UnityEngine;

public enum InterpolationTypes
{
    Linear,
    ConstantMin,
    // Hermite
}

[System.Serializable]
public class CustomGradient
{
    public ColorKeys ColorKeys = new ColorKeys();
    public InterpolationTypes interpolationTypes;

    public CustomGradient()
    {
        Init();
    }

    void Init()
    {
        ColorKeys.Add(Color.black, 0f);
        ColorKeys.Add(Color.white, 1f);
    }

    public void Reset()
    {
        ColorKeys.Clear();
        Init();
    }

    Color Evaluate(float time, InterpolationTypes type)
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

        Color color = new Color();
        switch (type)
        {
            case (InterpolationTypes.ConstantMin):
                {
                    color = ColorKeys[ti].Color;
                    
                    break;
                }
            case (InterpolationTypes.Linear):
                {
                    // InverseLerp: (t - ti) / (tii - ti)
                    float localTime = Mathf.InverseLerp(ColorKeys[ti].Time, ColorKeys[ti + 1].Time, time);
                    color = Color.Lerp(ColorKeys[ti].Color, ColorKeys[ti + 1].Color, localTime);
                    
                    break;
                }
        }

        return color;
    }

    public Texture2D GetTexture(int width, InterpolationTypes type = InterpolationTypes.Linear)
    {
        Texture2D texture = new Texture2D(width, 1);
        Color[] pixelColors = new Color[width];
        
        for (int i = 0; i < pixelColors.Length; i++)
        {
            float t = (float)i / (width - 1);
            pixelColors[i] = Evaluate(t, type);
        }
        
        texture.SetPixels(pixelColors);
        texture.Apply();

        return texture;
    }
}
