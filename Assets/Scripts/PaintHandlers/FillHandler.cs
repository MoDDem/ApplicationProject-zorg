using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillHandler : MonoBehaviour, IPaintTool
{
    private class PixelStack
    {
        private int w;
        private int h;
        private int[] stack;
        private int stackSize;
        private int stackPointer;

        public PixelStack(int width, int height) {
            w = width;
            h = height;
            stackSize = w * h;
            stack = new int[stackSize];
        }

        public bool Pop(ref int x, ref int y) {
            if (stackPointer > 0) {
                int p = stack[stackPointer];
                x = p / h;
                y = p % h;
                stackPointer--;
                return true;
            } else {
                return false;
            }
        }

        public bool Push(int x, int y) {
            if (stackPointer < stackSize - 1) {
                stackPointer++;
                stack[stackPointer] = h * x + y;
                return true;
            } else {
                return false;
            }
        }
    }
    private BitImage img;
    private Coroutine onlyOneMain;
    public bool GetIsActive() => GetComponent<Toggle>().isOn;
    
    public void FloodFill(BitImage _img, int x, int y, Color oldColor, Color newColor)
    {
        if(onlyOneMain != null) return;
        
        img = _img;
        onlyOneMain = StartCoroutine(Flood(x, y, oldColor, newColor));
    }
    
    private IEnumerator Flood(int x, int y, Color oldColor, Color newColor)
    {
        if (ColorFunc.IsEqualTo(oldColor, newColor) || 
            ColorFunc.IsEqualTo(oldColor, Color.black)) yield break;

        int w = img.paintedTexture.width;
        int h = img.paintedTexture.height;
        PixelStack stack = new PixelStack(w, h);

        int y1;
        bool spanLeft, spanRight;

        if (!stack.Push(x, y)) yield break;

        while (stack.Pop(ref x, ref y)) {
            y1 = y;
            while (y1 >= 0 && ColorFunc.IsEqualTo(img.GetPixelColor(x, y1), oldColor)) {
                y1--;
            }
            y1++;
            spanLeft = spanRight = false;
            while (y1 < h && ColorFunc.IsEqualTo(img.GetPixelColor(x, y1), oldColor)) {
                img.SetPixelColor(x, y1, newColor);
                if (!spanLeft && x > 0 && ColorFunc.IsEqualTo(img.GetPixelColor(x - 1, y1), oldColor)) {
                    if (!stack.Push(x - 1, y1)) yield break;
                    spanLeft = true;
                } else if (spanLeft && x > 0 && ColorFunc.IsEqualTo(img.GetPixelColor(x - 1, y1), oldColor)) {
                    spanLeft = false;
                }
                if (!spanRight && x < w - 1 && ColorFunc.IsEqualTo(img.GetPixelColor(x + 1, y1), oldColor)) {
                    if (!stack.Push(x + 1, y1)) yield break;
                    spanRight = true;
                } else if (spanRight && x < w - 1 && x < w && ColorFunc.IsEqualTo(img.GetPixelColor(x + 1, y1), oldColor)) {
                    spanRight = false;
                }
                y1++;
            }
        }
        img.SetNewPixelColor();
        onlyOneMain = null;
        //onPaintComplete?.Invoke(filledPixels);
    }
}
