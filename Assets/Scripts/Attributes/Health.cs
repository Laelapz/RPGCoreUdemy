using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        private float _healthPoints = -1f;

        private bool _isDead = false;
        private BaseStats _stats;

        private void Awake()
        {
            _stats = GetComponent<BaseStats>();
        }

        private void OnEnable()
        {
            if(_stats != null) _stats.OnLevelUpEvent += LevelUpHeal;
        }

        private void OnDisable()
        {
            if (_stats != null) _stats.OnLevelUpEvent -= LevelUpHeal;
        }

        private void Start()
        {
            if(GetCurrentHealth() < 0) _healthPoints = GetMaxHealth();
            if (transform.CompareTag("Player")) print("Current health from start: " + _healthPoints);
        }

        public bool IsDead()
        {
            return _isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0);

            if(GetCurrentHealth() == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public void LevelUpHeal()
        {
            HealHealth(_stats.GetStat(Stat.Health) - GetCurrentHealth());
        }

        public void HealHealth(float amount)
        {
            _healthPoints += amount;
        }

        public float GetPercentageHealth()
        {
            return 100 * (GetCurrentHealth() / GetMaxHealth());
        }
        
        public float GetCurrentHealth()
        {
            return _healthPoints;
        }

        public float GetMaxHealth()
        {
            return _stats.GetStat(Stat.Health);
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
            return _healthPoints;
        }

        public void RestoreState(object state)
        {
            if ((float)state == 0)
            {
                Die();
            }
            else
            {
                if(GetCurrentHealth() == 0)
                {
                    _isDead = false;
                    GetComponent<Animator>().SetTrigger("revive");
                    GetComponent<CapsuleCollider>().enabled = true;
                }
            }

            _healthPoints = (float)state;
            if (transform.CompareTag("Player")) print("Current health from RestoreState: " + _healthPoints);
        }
    }
}
