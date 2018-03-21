using UnityEngine;

[System.Serializable]
public class CustomGradient
{
    public Color Evaluate(float t)
    {
        return Color.Lerp(Color.black, Color.white, t);
    }

    public Texture2D GetTexture(int width)
    {
        Texture2D texture = new Texture2D(width, 1);
        Color[] pixelColors = new Color[width];
        
        for (int i = 0; i < pixelColors.Length; i++)
        {
            pixelColors[i] = Evaluate((float)i / (width - 1));
        }
        
        texture.SetPixels(pixelColors);
        texture.Apply();

        return texture;
    }
}
