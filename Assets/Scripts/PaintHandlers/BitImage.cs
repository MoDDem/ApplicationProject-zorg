using System;
using UnityEngine;
using UnityEngine.UI;
using Screen = UnityEngine.Device.Screen;

public class BitImage : MonoBehaviour
{
    public RawImage img;
    public RectTransform imgRect;
    public Color[] pixels;
    public Color[] pixelsMain;
    
    private Texture2D texture;
    public Texture2D paintedTexture;
    
    private void Awake()
    {
        texture = (Texture2D) img.mainTexture;
        paintedTexture = new Texture2D(texture.width, texture.height);
        
        pixels = texture.GetPixels();
        pixelsMain = new Color[texture.width * texture.height];
        Array.Copy(pixels, pixelsMain, texture.width * texture.height);
        
        paintedTexture.SetPixels(pixels);

        Debug.Log($"{texture.width} {texture.height}");
        
        imgRect = img.rectTransform;
    }

    public Color GetPixelColor(int x, int y) => pixels[x + y * texture.width];
    public Color GetMainPixelColor(int x, int y) => pixelsMain[x + y * texture.width];
    public void SetPixelColor(int x, int y, Color color) => pixels[x + y * texture.width] = color;

    public void SetNewPixelColor()
    {
        paintedTexture.SetPixels(pixels);
        paintedTexture.Apply();
        img.texture = paintedTexture;
    }
}
