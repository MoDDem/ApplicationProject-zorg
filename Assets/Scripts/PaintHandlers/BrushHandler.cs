using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushHandler : MonoBehaviour, IPaintTool
{
    private BitImage img;
    private Vector2 _prevPosition;
    private Coroutine onlyMainCoroutine;

    [SerializeField] private int radius = 5;
    public bool GetIsActive() => GetComponent<Toggle>().isOn;

    public void BrushFill(BitImage _img, int x, int y, Color newColor)
    {
        img = _img;
        var pos = new Vector2(x, y);
        
        PaintPixelRadius(x, y, newColor);
        if (_prevPosition == Vector2.zero)
        {
            _prevPosition = pos;
            return;
        }
        
        StartCoroutine(BrushLerp(_prevPosition, pos, newColor));
    }

    private IEnumerator BrushLerp(Vector2 prevPosition, Vector2 endPosition, Color newColor)
    {
        _prevPosition = endPosition;
        
        var lineLength = Vector2.Distance(prevPosition, endPosition);
        const int lerpCountAdjustNum = 40;
        var lerpCount = Mathf.CeilToInt(lineLength / lerpCountAdjustNum);
        
        for (int i = 1; i <= lerpCount; i++)
        {
            float lerpWeight = (float) i / lerpCount;
            var lerpPosition = Vector2.Lerp(prevPosition, endPosition, lerpWeight);
            
            PaintPixelRadius((int) lerpPosition.x, (int) lerpPosition.y, newColor);
            yield return null;
        }
    }
    
    private void PaintPixelRadius(int x, int y, Color newColor)
    {
        for (int _x = x - radius; _x <= x + radius; _x++)
        {
            for (int _y = y - radius; _y <= y + radius; _y++)
            {
                if (!img.CheckValidPoint(_x, _y) || img.GetPixelColor(_x, _y) == newColor)
                    continue;
                Color color;
                if (!ColorFunc.IsEqualTo(img.GetMainPixelColor(_x, _y), Color.black, 0.1f))
                    color = newColor;
                else
                    color = img.GetPixelColor(_x, _y);

                img.SetPixelColor(_x, _y, color);
            }
        }
        img.SetNewPixelColor();
    }
    
    public void ResetPosition()
    {
        _prevPosition = Vector2.zero;
    }
}
