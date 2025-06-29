using System;
using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform followTransform;
        [SerializeField] private float zPos;
        
        private Vector3 position;
        
        private void LateUpdate()
        {
            position = followTransform.position;
            position.z = zPos;

            transform.position = position;
        }
    }
}