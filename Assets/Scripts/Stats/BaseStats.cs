using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private ProgressionSO progression = null;
        [SerializeField] private GameObject levelUpParticleEffect = null;
        [SerializeField] private bool _shouldUseModifiers = false;
        private Experience experienceComponent = null;

        public event Action OnLevelUpEvent;

        private int currentLevel = 0;

        private void Awake()
        {
            experienceComponent = GetComponent<Experience>();
        }

        private void Start()
        {
            currentLevel = CalculateLevel();
        }

        private void OnEnable()
        {
            if (experienceComponent != null) experienceComponent.OnExpercienceGained += UpdateLevel;
        }

        private void OnDisable()
        {
            if (experienceComponent != null) experienceComponent.OnExpercienceGained -= UpdateLevel;

        }

        private void UpdateLevel()
        {
            print("Calculate level");
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                OnLevelUpEvent();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            if (progression != null) return (GetBaseStat(stat) + GetAdditiveModifiers(stat)) * (1 + GetPercentageModifier(stat) / 100);

            return 1;
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifiers(Stat stat)
        {
            if (!_shouldUseModifiers) return 0;

            float result = 0;
            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifier in provider.GetAdditiveModifiers(stat))
                {
                    result += modifier;
                }
            }
            return result;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!_shouldUseModifiers) return 0;

            float result = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    result += modifier;
                }
            }
            return result;
        }

        public int GetLevel()
        {
            if (currentLevel < 1) currentLevel = CalculateLevel();

            return currentLevel;
        }

        private int CalculateLevel()
        {
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
