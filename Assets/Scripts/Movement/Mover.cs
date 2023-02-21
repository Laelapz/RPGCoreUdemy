using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using System;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private Transform targetPosition;
        [SerializeField] private float maxNavPathLength = 40f;

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

        public bool CanMoveTo(Vector3 position)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, position, NavMesh.AllAreas, path);

            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            Vector3 lastPos = transform.position;
            float totalDistance = 0f;
            foreach (Vector3 pos in path.corners)
            {
                float distance = (lastPos - pos).magnitude;
                totalDistance += distance;
                lastPos = pos;
            }

            return totalDistance;
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
