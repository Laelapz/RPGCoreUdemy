using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private Health _target = null;
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

        public void SetTarget(Health target, float damage, bool isHoming)
        {
            _target = target;
            _damage = damage;
            _isHoming = isHoming;
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
            _target.TakeDamage(_damage);

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
