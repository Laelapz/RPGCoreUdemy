using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private Transform targetPosition;

        private NavMeshAgent _myNavMeshAgent;
        private Animator _myAnimator;
        private Fighter _fighter;

        private void Start()
        {
            _myNavMeshAgent = GetComponent<NavMeshAgent>();
            _myAnimator = GetComponent<Animator>();
            _fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            UpdateAnimator();

        }

        public void StartMoveAction(Vector3 destination)
        {
            _fighter.Cancel();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            _myNavMeshAgent.destination = destination;
            _myNavMeshAgent.isStopped = false;
        }

        public void Stop()
        {
            _myNavMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = _myNavMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            _myAnimator.SetFloat("forwardSpeed", localVelocity.z);
        }
    }
}
