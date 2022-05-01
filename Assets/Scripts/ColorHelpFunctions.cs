using System;
using UnityEngine;

public static class ColorHelpFunctions
{
    public static bool IsEqualTo(Color a, Color b, float tolerance)
    {
        return Math.Abs(a.r - b.r) < tolerance && 
               Math.Abs(a.g - b.g) < tolerance && 
               Math.Abs(a.b - b.b) < tolerance;
    }
}
