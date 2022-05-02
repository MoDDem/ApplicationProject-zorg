using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushHandler : MonoBehaviour, IPaintTool
{
    private BitImage img;
    private Vector2 _prevPosition;
    
    [SerializeField] private int radius = 5;
    public bool GetIsActive() => GetComponent<Toggle>().isOn;
    public void BrushFill(BitImage _img, int x, int y, Color newColor)
    {
        img = _img;
        
        if (_prevPosition == Vector2.zero)
            _prevPosition = new Vector2(x, y);

        Vector2 endPosition = new Vector2(x, y);
        var lineLength = Vector2.Distance(_prevPosition, endPosition);
        const int lerpCountAdjustNum = 3;
        var lerpCount = Mathf.CeilToInt(lineLength / lerpCountAdjustNum);

        StartCoroutine(Brush(_prevPosition, endPosition, lerpCount, newColor));
    }

    public void ResetPosition()
    {
        if (_prevPosition.Equals(Vector2.zero))
            return;
        
        _prevPosition = Vector2.zero;
    }
    
    private IEnumerator Brush(Vector2 prevPosition, Vector2 endPosition, int lerpCount, Color newColor)
    {
        for (int i = 1; i <= lerpCount; i++)
        {
            float lerpWeight = (float) i / lerpCount;

            var lerpPosition = Vector2.Lerp(prevPosition, endPosition, lerpWeight);
            
            PaintPixelRadius((int) lerpPosition.x, (int) lerpPosition.y, newColor);
        }
        _prevPosition = endPosition;
        img.SetNewPixelColor();
        yield break;
    }

    private void PaintPixelRadius(int x, int y, Color newColor)
    {
        for (int _x = x - radius; _x <= x + radius; _x++)
        {
            for (int _y = y - radius; _y <= y + radius; _y++)
            {
                if (!img.CheckValidPoint(_x, _y))
                    continue;
                
                Color color;
                if (!ColorHelpFunctions.IsEqualTo(img.GetMainPixelColor(_x, _y), Color.black, 0.1f))
                    color = newColor;
                else
                    color = img.GetPixelColor(_x, _y);

                img.SetPixelColor(_x, _y, color);
            }
        }
        _prevPosition = Vector2.zero;
    }
}
