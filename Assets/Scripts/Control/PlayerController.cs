using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Mover _mover;
        [SerializeField] private Fighter _fighter;
        private Health _health;

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            if (_health.IsDead()) return;

            if (InteractWithCombat()) return;

            if(InteractWithMovement()) return;
        }


        private bool InteractWithCombat()
        {
            
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                CombatTarget combatTargetScript = hit.collider.GetComponent<CombatTarget>();
                if (combatTargetScript == null) continue;

                if (!_fighter.CanAttack(combatTargetScript.gameObject)) continue;

                if (Input.GetMouseButtonDown(0))
                {

                    _fighter.Attack(combatTargetScript.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }

        private Ray GetMouseRay()
        {
            return _mainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
