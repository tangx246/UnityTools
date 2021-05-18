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
            SetTextHelper(arg0);
        }

        public void SetText(int _, int newValue)
        {
            SetTextHelper(newValue);
        }

        public void SetText(float arg0)
        {
            SetTextHelper(arg0);
        }

        public void SetText(float _, float newValue)
        {
            SetTextHelper(newValue);
        }

        // Unity Editor does not recognize type object, so we overload public methods
        private void SetTextHelper(object arg0)
        {
            textBox.SetText(string.Format(text, arg0));
        }
    }
}