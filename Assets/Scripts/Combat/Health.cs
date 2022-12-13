using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        public float health = 100f;

        public void TakeDamage(float damage)
        {
            health -= damage;

            if(health <= 0)
            {
                health = 0f;
                print("Inimigo morreu");
            }
        }
    }
}
