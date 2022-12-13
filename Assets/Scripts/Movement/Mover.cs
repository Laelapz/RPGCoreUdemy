using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] private Transform targetPosition;

        private ActionScheduler _actionScheduler;

        private NavMeshAgent _myNavMeshAgent;
        private Animator _myAnimator;

        private void Start()
        {
            _myNavMeshAgent = GetComponent<NavMeshAgent>();
            _myAnimator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            _actionScheduler.StartAction(this);
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

        public void Cancel()
        {
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = _myNavMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            _myAnimator.SetFloat("forwardSpeed", localVelocity.z);
        }
    }
}
