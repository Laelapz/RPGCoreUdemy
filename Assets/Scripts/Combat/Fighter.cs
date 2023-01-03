using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float _timeBetweenAttacks = 1f;
        [SerializeField] private Transform handTransform = null;
        [SerializeField] private WeaponSO defaultWeapon = null;
        private WeaponSO currentWeapon = null;

        private ActionScheduler _actionScheduler;
        private Animator _animator;

        private Health _target;
        private Mover _mover;

        private float _timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
            EquipWeapon(defaultWeapon);
        }

        public void EquipWeapon(WeaponSO weapon)
        {
            currentWeapon = weapon;

            weapon.Spawn(handTransform, _animator);
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

            _target.TakeDamage(currentWeapon.WeaponDamage);
        }
    }
}
