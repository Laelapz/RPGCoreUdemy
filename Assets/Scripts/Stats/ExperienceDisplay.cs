using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private TMP_Text experienceTextValue;
        Experience experience;

        private void Awake()
        {
            experienceTextValue = GetComponent<TMP_Text>();
            experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            experienceTextValue.text = experience.ExperiencePoints.ToString();
        }
    }
}
