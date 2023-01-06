using RPG.Attributes;
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

        [SerializeField] private bool _isRightHanded = true;
        [SerializeField] private bool _isHoming = false;
        [SerializeField] private Projectile projectile = null;

        const string weaponName = "Weapon";

        public void Spawn(Transform rightHand, Transform leftHand,  Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if(equippedPrefab != null)
            {
                Transform hand;

                hand = GetTransform(rightHand, leftHand);

                GameObject weapon = Instantiate(equippedPrefab, hand);
                weapon.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if(oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if(oldWeapon == null) return;

            oldWeapon.name = "Destroying";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            return _isRightHanded ? rightHand : leftHand;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, _weaponDamage, _isHoming);
        }
    }
}