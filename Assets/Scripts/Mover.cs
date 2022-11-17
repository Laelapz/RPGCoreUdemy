using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;

    private NavMeshAgent _myNavMeshAgent;
    private Animator _myAnimator;
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
        _myNavMeshAgent = GetComponent<NavMeshAgent>();
        _myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
        }

        UpdateAnimator();

    }

    private void MoveToCursor()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool hasHit = Physics.Raycast(ray, out hit);

        if (hasHit)
        {
            _myNavMeshAgent.destination = hit.point;

        }
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = _myNavMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        _myAnimator.SetFloat("forwardSpeed", localVelocity.z);
    }
}
