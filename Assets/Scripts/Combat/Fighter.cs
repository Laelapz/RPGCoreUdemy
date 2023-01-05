using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;
using System.Security.Cryptography;
using RPG.Saving;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private float _timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private WeaponSO defaultWeapon = null;
        private WeaponSO currentWeapon = null;

        private ActionScheduler _actionScheduler;
        private Animator _animator;

        private Health _target;
        private Mover _mover;

        private float _timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
            
            if(currentWeapon == null) EquipWeapon(defaultWeapon);
        }

        public void EquipWeapon(WeaponSO weapon)
        {
            currentWeapon = weapon;

            weapon.Spawn(rightHandTransform, leftHandTransform, _animator);
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target == null) return;
            if (_target.IsDead()) return;

            if (!GetIsInRange())
            {
                _mover.MoveTo(_target.transform.position, 1f);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            Health _healthScript = combatTarget.GetComponent<Health>();
            return (_healthScript != null && !_healthScript.IsDead());
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);

            if (_timeSinceLastAttack < _timeBetweenAttacks) return;
            TriggerAttack();
            _timeSinceLastAttack = 0f;
        }

        private void TriggerAttack()
        {
            _animator.ResetTrigger("stopAttack");
            _animator.SetTrigger("attack");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) < currentWeapon.WeaponRange;
        }

        public void Attack(GameObject combatTarget)
        {
            _actionScheduler.StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            _mover.Cancel();
            _target = null;
        }

        private void StopAttack()
        {
            _animator.ResetTrigger("attack");
            _animator.SetTrigger("stopAttack");
        }

        //Animation Event
        private void Hit()
        {
            if (_target == null) return;

            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, _target);
            }
            else
            {
                _target.TakeDamage(currentWeapon.WeaponDamage);
            }
        }

        private void Shoot()
        {
            Hit();
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponSO weapon = Resources.Load<WeaponSO>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
