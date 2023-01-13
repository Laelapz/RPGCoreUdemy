using TMPro;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        private TextMeshProUGUI _damageText = null;

        private void Awake()
        {
            _damageText = GetComponentInChildren<TextMeshProUGUI>(); 
        }

        public void SetText(string text)
        {
            _damageText.text = text;
        }

        public void DestroyText()
        {
            Destroy(gameObject);
        }
    }

}