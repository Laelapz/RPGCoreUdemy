using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        [SerializeField] private bool _isHoming = false;
        [SerializeField] private GameObject hitEffect = null;

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
            var direction = Vector3.forward * speed * Time.deltaTime;
            transform.Translate(direction);
        }

        public void SetTarget(Health target, float damage, bool isHoming)
        {
            _target = target;
            _damage = damage;
            _isHoming = isHoming;
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
            if(hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
