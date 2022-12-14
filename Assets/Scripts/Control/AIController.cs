using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {

        [SerializeField] private float _chaseDistance = 5f;
        private GameObject _player;
        private Health _health;
        private Fighter _fighter;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            if (_health.IsDead()) return;

            if (InAttackRangeOfPlayer() && _fighter.CanAttack(_player))
            {
                _fighter.Attack(_player);
            }
            else
            {
                _fighter.Cancel();
            }

        }

        private bool InAttackRangeOfPlayer()
        {
            return _player != null && Vector3.Distance(transform.position, _player.transform.position) < _chaseDistance;
        }
    }
}
