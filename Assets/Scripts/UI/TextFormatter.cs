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

        public void SetText(object arg0)
        {
            textBox.SetText(string.Format(text, arg0));
        }
    }
}