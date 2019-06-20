using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGenerator
{

    ColourSettings settings;
    Texture2D texture;
    const int textureResolution = 650;

    public bool IsActiveTextureNull()
    {
        if (texture == null || settings.planetMaterial.GetTexture("_texture") == null)
            return true;
        else
            return false;
    }

    public void UpdateSettings(ColourSettings settings)
    {
        if (this.settings != settings)
            this.settings = settings;

        if (texture == null && settings.planetMaterial.GetTexture("_texture") != null)
        {
            texture = (Texture2D)settings.planetMaterial.GetTexture("_texture");
        }
        else if (texture != null && settings.planetMaterial.GetTexture("_texture") == null)
        {
            UpdateColours();
        }
        else
        {
            texture = new Texture2D(textureResolution, 1);
        }
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public void UpdateColours()
    {
        Color[] colours = new Color[textureResolution];
        for (int i = 0; i < textureResolution; i++)
        {
            colours[i] = settings.gradient.Evaluate(i / (textureResolution - 1f));
        }
        texture.SetPixels(colours);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }
}
