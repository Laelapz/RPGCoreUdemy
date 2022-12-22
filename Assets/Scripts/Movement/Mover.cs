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
        private Health _health;
        
        private float _maxSpeed = 6.3f;

        private void Start()
        {
            _myNavMeshAgent = GetComponent<NavMeshAgent>();
            _myAnimator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            _myNavMeshAgent.enabled = !_health.IsDead();

            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _myNavMeshAgent.destination = destination;
            _myNavMeshAgent.speed = _maxSpeed * Mathf.Clamp01(speedFraction); ;
            _myNavMeshAgent.isStopped = false;
        }

        public void Cancel()
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
