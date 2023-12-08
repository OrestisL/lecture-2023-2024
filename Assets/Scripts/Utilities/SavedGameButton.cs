using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavedGameButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI text;

    public void Init(Action onClicked, string text)
    {
        button.onClick.AddListener(() => onClicked?.Invoke());
        this.text.text = text;
    }
}
