using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using UnityEngine;
using GameDevTV.Utils;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        private LazyValue<float> _healthPoints;

        private bool _isDead = false;
        private BaseStats _stats;

        private void Awake()
        {
            _stats = GetComponent<BaseStats>();
            _healthPoints = new LazyValue<float>(GetMaxHealth);
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
            _healthPoints.ForceInit();
        }

        public bool IsDead()
        {
            return _isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints.value = Mathf.Max(_healthPoints.value - damage, 0);

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
            _healthPoints.value += amount;
        }

        public float GetPercentageHealth()
        {
            return 100 * (GetCurrentHealth() / GetMaxHealth());
        }
        
        public float GetCurrentHealth()
        {
            return _healthPoints.value;
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
            return _healthPoints.value;
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

            _healthPoints.value = (float)state;
            if (transform.CompareTag("Player")) print("Current health from RestoreState: " + _healthPoints);
        }
    }
}
