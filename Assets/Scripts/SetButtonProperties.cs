using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class SetButtonProperties : MonoBehaviour
{
    private Image background, icon;
    private TextMeshProUGUI label;
    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        background = GetComponent<Image>();
        icon = GetComponentInChildren<Image>();
        label = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        icon.color = background.color;
    }

    public void SetProperties(string text, Sprite sprite, Action onClicked = null) 
    {
        label.text = text;
        icon.sprite = sprite;
        if (onClicked != null)
            button.onClick.AddListener(() => onClicked.Invoke());
    }
}

