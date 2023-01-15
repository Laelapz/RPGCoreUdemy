using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using System;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private Transform targetPosition;


        private ActionScheduler _actionScheduler;

        private NavMeshAgent _myNavMeshAgent;
        private Animator _myAnimator;
        private Health _health;
        
        private float _maxSpeed = 6.3f;

        private void Awake()
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

        public object CaptureState()
        {
            MoverSaveData moverSaveData = new MoverSaveData();
            moverSaveData.position = new SerializableVector3(transform.position);
            moverSaveData.rotation = new SerializableVector3(transform.eulerAngles);
            return moverSaveData;
        }

        public void RestoreState(object state)
        {
            MoverSaveData moverSaveData = (MoverSaveData)state;
            GetComponent<NavMeshAgent>().Warp(moverSaveData.position.ToVector());
            transform.eulerAngles = moverSaveData.rotation.ToVector();
        }

        [Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }
    }
}
