using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float _weaponDamage = 5f;
        [SerializeField] private float _weaponRange = 2f;
        [SerializeField] private float _timeBetweenAttacks = 1f;

        private ActionScheduler _actionScheduler;
        private Animator _animator;

        private Transform _target;
        private Mover _mover;

        private float _timeSinceLastAttack = 0f;

        private void Start()
        {
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target == null) return;

            if (!GetIsInRange())
            {
                _mover.MoveTo(_target.position);
            }
            else
            {
                _mover.Stop();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if (_timeSinceLastAttack < _timeBetweenAttacks) return;
         
            _animator.SetTrigger("attack");
            _timeSinceLastAttack = 0f;
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.position) < _weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            _actionScheduler.StartAction(this);
            _target = combatTarget.transform;
            print("Take that you short, squat peasant!");
        }

        public void Cancel()
        {
            _target = null;
        }

        //Animation Event
        private void Hit()
        {
            Health enemyHealth = _target.GetComponent<Health>();

            if (enemyHealth != null) enemyHealth.TakeDamage(_weaponDamage);
        }
    }
}
