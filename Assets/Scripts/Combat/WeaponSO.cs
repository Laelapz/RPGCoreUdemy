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

        [SerializeField] private bool isRightHanded = true;

        public void Spawn(Transform rightHand, Transform leftHand,  Animator animator)
        {
            if(equippedPrefab != null)
            {
                Transform hand;

                hand = isRightHanded ? rightHand : leftHand;

                Instantiate(equippedPrefab, hand);
            }

            if(animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }
    }
}