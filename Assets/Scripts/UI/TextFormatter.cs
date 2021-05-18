using TMPro;
using UnityEngine;

namespace UnityTools
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextFormatter : MonoBehaviour
    {
        private TextMeshProUGUI textBox;

        public string text;

        void Awake()
        {
            textBox = GetComponent<TextMeshProUGUI>();
        }

        public void SetText(int arg0)
        {
            textBox.SetText(string.Format(text, arg0));
        }

        public void SetText(int _, int newValue)
        {
            textBox.SetText(string.Format(text, newValue));
        }

        public void SetText(float arg0)
        {
            textBox.SetText(string.Format(text, arg0));
        }

        public void SetText(float _, float newValue)
        {
            textBox.SetText(string.Format(text, newValue));
        }
    }
}