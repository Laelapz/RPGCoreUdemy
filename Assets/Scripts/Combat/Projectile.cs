using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float _speed = 12f;
        [SerializeField] private bool _isHoming = false;
        [SerializeField] private GameObject _hitEffect = null;
        [SerializeField] private float _maxLifeTime = 10f;
        [SerializeField] private GameObject[] _destroyOnHit;
        [SerializeField] private float _lifeAfterImpact = 2f;

        [SerializeField] UnityEvent onHit;

        private Health _target = null;
        private GameObject _instigator = null;
        private float _damage = 0;

        private void Start()
        {

            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (_target == null) return;

            if (!_target.IsDead() && _isHoming) transform.LookAt(GetAimLocation());
            var direction = Vector3.forward * _speed * Time.deltaTime;
            transform.Translate(direction);
        }

        public void SetTarget(Health target, GameObject instigator, float damage, bool isHoming)
        {
            _target = target;
            _damage = damage;
            _isHoming = isHoming;
            _instigator = instigator;
            Destroy(gameObject, _maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = _target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) return _target.transform.position;

            return _target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<Health>() != _target) return;
            _target.TakeDamage(_instigator, _damage);
            onHit.Invoke();

            _speed = 0f;

            if(_hitEffect != null)
            {
                Instantiate(_hitEffect, GetAimLocation(), Quaternion.identity);
            }

            foreach(GameObject toDestroy in _destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, _lifeAfterImpact);
        }
    }
}
