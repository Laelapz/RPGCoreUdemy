using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float experiencePoints = 0;

        public event Action OnExpercienceGained;

        public float ExperiencePoints => experiencePoints;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            OnExpercienceGained();
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
