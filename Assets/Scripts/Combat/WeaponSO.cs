using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponSO : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;

        [SerializeField] private float _weaponDamage = 5f;
        public float WeaponDamage => _weaponDamage;

        [SerializeField] private float _weaponRange = 2f;
        public float WeaponRange => _weaponRange;

        public void Spawn(Transform handTransform, Animator animator)
        {
            if(equippedPrefab != null)
            {
                Instantiate(equippedPrefab, handTransform);
            }

            if(animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }
    }
}