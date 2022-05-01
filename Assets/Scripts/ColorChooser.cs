using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class ColorChooser : MonoBehaviour
{
    public Color? selectedColor = null;
    
    [SerializeField] private GameObject colorPanel;
    [SerializeField] private Image bgColor;

    private InitializeColors colors;

    private void Awake()
    {
        colors = colorPanel.GetComponent<InitializeColors>();
        colors.colorChanged += ColorChanged;
    }

    private void Start() => ColorChanged(0);

    public void ChangePanelVisibility() => colorPanel.SetActive(!colorPanel.activeSelf);

    private void ColorChanged(int id)
    {
        selectedColor = colors.GetColorById(id);
        if(selectedColor != null)
            bgColor.color = (Color)selectedColor;
    }
}
