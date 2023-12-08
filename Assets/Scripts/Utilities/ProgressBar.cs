using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image fill;
    [SerializeField] private float width = 1000.0f;
    public float Progress
    {
        set
        {
            _progress = Mathf.Clamp01(value);
            fill.rectTransform.sizeDelta = new Vector2(_progress * width, fill.rectTransform.sizeDelta.y);
        }
        get
        {
            return _progress;
        }
    }
    private float _progress;

    public IEnumerator LoadingText()
    {
        int count = 0;
        while (true)
        {
            text.text += ".";
            count++;
            if (count == 5)
            {
                text.text = text.text.Remove(text.text.Length - (count + 1));
                count = 0;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
