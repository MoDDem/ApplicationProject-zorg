using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InitializeColors : MonoBehaviour
{
    [SerializeField] private int count = 10;
    private List<Color> rndColors = new List<Color>();
    public Action<int> colorChanged;

    private void Awake()
    {
        gameObject.SetActive(true);
        
        var scroll = gameObject.GetComponent<ScrollRect>();
        var template = scroll.content.GetChild(0);

        for (int i = 0; i < count; i++)
        {
            var clr = Random.ColorHSV();
            rndColors.Add(clr);
            
            var colorObject = Instantiate(template);
            
            colorObject.GetComponentInChildren<Image>().color = clr;

            var _i = i;
            var toggleComponent = colorObject.GetComponent<Toggle>();
            toggleComponent.onValueChanged.AddListener(
                _ => colorChanged?.Invoke(_i)
                );
            
            colorObject.gameObject.SetActive(true);
            
            var rect = colorObject.GetComponent<RectTransform>();
            rect.SetParent(scroll.content);
            rect.localScale = Vector3.one;
        }
        
        Destroy(template.gameObject);
        gameObject.SetActive(false);
    }

    public Color GetColorById(int id) => rndColors[id];
}
