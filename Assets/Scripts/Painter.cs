using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Screen = UnityEngine.Device.Screen;

public class Painter : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private ColorChooser colorChooser;
    [SerializeField] private BrushHandler brush;
    [SerializeField] private BrushHandler erase;
    [SerializeField] private FillHandler floodFill;
    private BitImage bitImage;

    private void Awake()
    {
        bitImage = GetComponent<BitImage>();
    }

    void Update()
    {
        var touch = Touchscreen.current.primaryTouch.ReadValue();
        var phase = touch.phase;
        
        if (touch.isInProgress && !colorChooser.GetActive())
        {
            var pos = TranslatePointToImagePoint();
            
            if (!bitImage.CheckValidPoint(pos.x, pos.y))
                return;
            
            if (brush.GetIsActive())
            {
                brush.BrushFill(bitImage, pos.x, pos.y, (Color) colorChooser.selectedColor);
            }
            else if (floodFill.GetIsActive())
            {
                floodFill.FloodFill(bitImage, pos.x, pos.y, 
                    bitImage.GetPixelColor(pos.x, pos.y), (Color) colorChooser.selectedColor);
            }
            else if (erase.GetIsActive())
            {
                erase.BrushFill(bitImage, pos.x, pos.y, Color.white);
            }
        }

        if (phase is TouchPhase.Ended or TouchPhase.Canceled)
        {
            brush.ResetPosition();
            erase.ResetPosition();
            touch.phase = TouchPhase.None;
        }
        
    }

    private Vector2Int TranslatePointToImagePoint()
    {
        var touch = Touchscreen.current.primaryTouch.ReadValue();
        var rect = RectTransformToScreenSpace(bitImage.imgRect);
        
        var minHeight = Screen.height - rect.height;
        var mapY = math.remap(minHeight, Screen.height, 0, bitImage.paintedTexture.height, touch.position.y);
        var mapX = math.remap(0, Screen.width, 0, bitImage.paintedTexture.width, touch.position.x);

        return new Vector2Int(Mathf.RoundToInt(mapX),Mathf.RoundToInt(mapY));
    }
    
    public Rect RectTransformToScreenSpace(RectTransform _transform)
    {
        Vector2 size= Vector2.Scale(_transform.rect.size, _transform.lossyScale);
        float x= _transform.position.x + _transform.anchoredPosition.x;
        float y= Screen.height - _transform.position.y - _transform.anchoredPosition.y;
 
        return new Rect(x, y, size.x, size.y);
    }
}
