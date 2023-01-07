using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        public float healthPoints = 100f;

        private bool _isDead = false;
        private BaseStats _stats;

        private void Start()
        {
            _stats = GetComponent<BaseStats>();
            healthPoints = _stats.GetStat(Stat.Health); ;
        }

        public bool IsDead()
        {
            return _isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if(healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }


        public float GetPercentageHealth()
        {
            return 100 * (healthPoints / _stats.GetStat(Stat.Health));
        }

        private void Die()
        {
            if (_isDead) return;

            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<CapsuleCollider>().enabled = false;
            _isDead = true;
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (!experience) return;

            experience.GainExperience(_stats.GetStat(Stat.ExperienceReward));
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            if ((float)state == 0)
            {
                Die();
            }
            else
            {
                if(healthPoints == 0)
                {
                    _isDead = false;
                    GetComponent<Animator>().SetTrigger("revive");
                    GetComponent<CapsuleCollider>().enabled = true;
                }
            }

            healthPoints = (float)state;
        }
    }
}
