using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        public float healthPoints = 100f;

        private bool _isDead = false;

        public bool IsDead()
        {
            return _isDead;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if(healthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (_isDead) return;

            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            _isDead = true;
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
                }
            }

            healthPoints = (float)state;
        }
    }
}
