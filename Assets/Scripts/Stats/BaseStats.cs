using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] ProgressionSO progression = null;

        public float GetHealth()
        {
            if(progression != null) return progression.GetHealth(characterClass, startingLevel);

            return 1;
        }

        public float GetExperienceReward()
        {
            return 10;
        }
    }
}
