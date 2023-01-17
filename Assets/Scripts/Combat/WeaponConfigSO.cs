using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfigSO : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedPrefab = null;

        [SerializeField] private float _weaponDamage = 5f;
        public float WeaponDamage => _weaponDamage;


        [SerializeField] private float _percentageBonus = 10f;
        public float PercentageBonus => _percentageBonus;

        [SerializeField] private float _weaponRange = 2f;
        public float WeaponRange => _weaponRange;

        [SerializeField] private bool _isRightHanded = true;
        [SerializeField] private bool _isHoming = false;
        [SerializeField] private Projectile projectile = null;

        const string weaponName = "Weapon";

        public Weapon Spawn(Transform rightHand, Transform leftHand,  Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            Weapon weapon = null;

            if(equippedPrefab != null)
            {
                Transform hand;

                hand = GetTransform(rightHand, leftHand);

                weapon = Instantiate(equippedPrefab, hand);
                weapon.gameObject.name = weaponName;
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

            return weapon;
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

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);

            //Adicionar o dano extra da arma "currentWeapon.WeaponDamage"
            projectileInstance.SetTarget(target, instigator, calculatedDamage, _isHoming);
        }
    }
}