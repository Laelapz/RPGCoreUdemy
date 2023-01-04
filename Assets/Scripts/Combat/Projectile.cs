using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] float speed = 1f;
    

    void Update()
    {
        transform.LookAt(_target);
        var direction = Vector3.forward * speed * Time.deltaTime;
        transform.Translate(direction);
    }
}
