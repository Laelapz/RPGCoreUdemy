using RPG.Attributes;
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

        private void Update()
        {
            if (gameObject.CompareTag("Player"))
            {
                print(GetLevel());
            }
        }

        public float GetStat(Stat stat)
        {
            if(progression != null) return progression.GetStat(stat, characterClass, GetLevel());

            return 1;
        }

        public int GetLevel()
        {
            Experience experienceComponent = GetComponent<Experience>();
            if (experienceComponent == null) return startingLevel;

            float currentXP = experienceComponent.ExperiencePoints;
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level < penultimateLevel; level++)
            {
                float xpToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if(xpToLevelUp > currentXP) return level;
            }

            return penultimateLevel + 1;
        }
    }
}
