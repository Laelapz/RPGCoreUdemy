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

        public float GetStat(Stat stat)
        {
            if(progression != null) return progression.GetStat(stat, characterClass, startingLevel);

            return 1;
        }
    }
}
