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
        if(!RectTransformUtility.RectangleContainsScreenPoint(bitImage.imgRect, 
               Touchscreen.current.primaryTouch.ReadValue().position)) return;

        if (Touchscreen.current.primaryTouch.ReadValue().phase is TouchPhase.Began or TouchPhase.Moved)
        {
            var pos = TranslatePointToImagePoint();

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
                brush.BrushFillErase(bitImage, pos.x, pos.y);
            }
        }
    }

    private Vector2Int TranslatePointToImagePoint()
    {
        Vector2 proportion = Vector2.one / canvas.transform.localScale;

        var touch = Touchscreen.current.primaryTouch.ReadValue();
        
        Vector2 touchPointing = touch.position * proportion;
        touchPointing.y -= (Screen.height - bitImage.imgRect.rect.height * 2) * proportion.y;

        var pos = new Vector2Int(
            (int)math.remap(0, bitImage.imgRect.rect.width, 0, bitImage.paintedTexture.width, touchPointing.x),
            (int)math.remap(0, bitImage.imgRect.rect.height, 0, bitImage.paintedTexture.height, touchPointing.y)
        );

        return pos;
    }
}
