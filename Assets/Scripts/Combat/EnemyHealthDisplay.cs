using RPG.Attributes;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private TMP_Text healthTextValue;
        Fighter fighter;

        private void Awake()
        {
            healthTextValue = GetComponent<TMP_Text>();
            fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            Health target = fighter.GetTarget();

            if (!target || target.IsDead())
            {
                healthTextValue.text = "N/A";
            }
            else
            {
                healthTextValue.text = string.Format("{0:0}%", target.GetPercentageHealth());
            }
        }
    }
}
