using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] private float _timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private WeaponConfigSO defaultWeaponConfig = null;
        private WeaponConfigSO currentWeaponConfig = null;
        private LazyValue<Weapon> currentWeapon;

        private ActionScheduler _actionScheduler;
        private Animator _animator;

        private Health _target;
        private Mover _mover;
        private BaseStats _baseStats;

        private float _timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
            _baseStats = GetComponent<BaseStats>();

            currentWeaponConfig = defaultWeaponConfig;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeaponConfig);
        }

        public void EquipWeapon(WeaponConfigSO weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfigSO weapon)
        {
            return weapon.Spawn(rightHandTransform, leftHandTransform, _animator);
        }

        public Health GetTarget()
        {
            return _target;
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
            return Vector3.Distance(transform.position, _target.transform.position) < currentWeaponConfig.WeaponRange;
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

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage) yield return currentWeaponConfig.WeaponDamage;
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage) yield return currentWeaponConfig.PercentageBonus;
        }

        //Animation Event
        private void Hit()
        {
            if (_target == null) return;

            //
            //Adicionar o dano extra da arma nos dois ataques "currentWeapon.WeaponDamage"
            //

            if (currentWeapon.value != null) currentWeapon.value.OnHit();

            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, _target, gameObject, _baseStats.GetStat(Stat.Damage));
            }
            else
            {
                _target.TakeDamage(gameObject, _baseStats.GetStat(Stat.Damage));
            }
        }

        private void Shoot()
        {
            Hit();
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfigSO weapon = Resources.Load<WeaponConfigSO>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
