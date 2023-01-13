using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        private Camera _mainCamera;
        
        void Awake()
        {
            _mainCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            transform.forward = _mainCamera.transform.forward;
        }
    }
}
