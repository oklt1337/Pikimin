using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Camera
{
    public class CameraMovement : MonoBehaviour
    {
        [Header("Camera Stats")]
        public Transform target;
        
        [SerializeField]private float smoothing;

        private void Start()
        {
            var targetPosition = target.position;
            var myTransform = transform;
            
            myTransform.position = new Vector3(targetPosition.x,targetPosition.y, myTransform.position.z);
        }

        private void LateUpdate()
        {
            SmoothCameraMovement(target);
        }

        /// <summary>
        /// Follows the my target smoothly.
        /// </summary>
        private void SmoothCameraMovement(Transform myTarget)
        {
            var position = myTarget.position;
            var selfPosition = transform.position;
            var targetPosition = new Vector3(position.x, position.y, selfPosition.z);
            selfPosition = Vector3.Lerp(selfPosition, targetPosition, smoothing);
            
            transform.position = selfPosition;
        }
    }
}
