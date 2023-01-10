using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        private TMP_Text healthTextValue;
        Health health;

        private void Awake()
        {
            healthTextValue = GetComponent<TMP_Text>();
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            healthTextValue.text = string.Format("{0:0}/{1:0}", health.GetCurrentHealth(), health.GetMaxHealth());
        }
    }
}
