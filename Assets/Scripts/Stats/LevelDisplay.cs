using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        private TMP_Text experienceLevelTextValue;
        BaseStats experienceLevel;

        private void Awake()
        {
            experienceLevelTextValue = GetComponent<TMP_Text>();
            experienceLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            experienceLevelTextValue.text = experienceLevel.GetLevel().ToString();
        }
    }
}
