using System;
using UnityEngine;

public static class ColorFunc
{
    public static bool IsEqualTo(Color a, Color b, float tolerance = 0.1f)
    {
        return Math.Abs(a.r - b.r) < tolerance && 
               Math.Abs(a.g - b.g) < tolerance && 
               Math.Abs(a.b - b.b) < tolerance;
    }
}
