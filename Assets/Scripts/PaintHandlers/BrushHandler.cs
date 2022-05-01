using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushHandler : MonoBehaviour, IPaintTool
{
    private BitImage img;
    private Vector2 prevPos = Vector2.zero;
    
    [SerializeField] private int radius = 5;
    public bool GetIsActive() => GetComponent<Toggle>().isOn;
    public void BrushFill(BitImage _img, int x, int y, Color newColor)
    {
        img = _img;

        StartCoroutine(Brush(x, y, newColor));
    }

    private IEnumerator Brush(float _x, float _y, Color newColor)
    {
        int center_x = (int) _x;
        int center_y = (int) _y;

        for (int x = center_x - radius; x <= center_x + radius; x++)
        {
            for (int y = center_y - radius; y <= center_y + radius; y++)
            {
                if (!ColorHelpFunctions.IsEqualTo(img.GetPixelColor(x,y), Color.black, 0.1f))
                    img.SetPixelColor(x, y, newColor);
                else
                    img.SetPixelColor(x, y, img.GetPixelColor(x,y));
            }
        }
        
        img.SetNewPixelColor();
        yield break;
    }
    
    public void BrushFillErase(BitImage _img, int x, int y)
    {
        img = _img;

        StartCoroutine(BrushErase(x, y));
    }

    private IEnumerator BrushErase(float _x, float _y)
    {
        int center_x = (int) _x;
        int center_y = (int) _y;

        for (int x = center_x - radius; x <= center_x + radius; x++)
        {
            for (int y = center_y - radius; y <= center_y + radius; y++)
            {
                img.SetPixelColor(x, y, img.GetMainPixelColor(x,y));
            }
        }
        
        img.SetNewPixelColor();
        yield break;
    }
}
