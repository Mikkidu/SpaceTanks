using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexDev.SpaceTanks
{
    public class CameraWork : MonoBehaviour
    {

        [Tooltip("The distance in the local x-z plane to the target")]
        [SerializeField]
        private float distance = 7.0f;

        [Tooltip("Allow the camera to be offseted vertically from the target, for example giving more view of the sceneray and less ground.")]
        [SerializeField]
        private Vector3 centerOffset = Vector3.zero;

        [Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
        [SerializeField]
        private bool followOnStart = false;

        [Tooltip("The Smoothing for the camera to follow the target")]
        [SerializeField]
        private float smoothSpeed = 0.125f;

        // cached transform of the target
        private Transform cameraTransform;

        // maintain a flag internally to reconnect if target is lost or camera is switched
        private bool isFollowing;

        // Cache for camera offset
        private Vector3 cameraOffset = Vector3.zero;


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase
        /// </summary>
        void Start()
        {
            // Start following the target if wanted.
            if (followOnStart)
            {
                OnStartFollowing();
            }
        }


        void LateUpdate()
        {
            // The transform target may not destroy on level load,
            // so we need to cover corner cases where the Main Camera is different everytime we load a new scene, and reconnect when that happens
            if (cameraTransform == null && isFollowing)
            {
                OnStartFollowing();
            }

            // only follow is explicitly declared
            if (isFollowing)
            {
                Follow();
            }
        }

        /// <summary>
        /// Raises the start following event.
        /// Use this when you don't know at the time of editing what to follow, typically instances managed by the photon network.
        /// </summary>
        public void OnStartFollowing()
        {
            cameraTransform = Camera.main.transform;
            isFollowing = true;
            // we don't smooth anything, we go straight to the right camera shot
            Cut();
        }

        /// <summary>
        /// Follow the target smoothly
        /// </summary>
        void Follow()
        {
            Vector3 newCameraPosition = transform.position + cameraOffset + centerOffset;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, newCameraPosition, smoothSpeed * Time.deltaTime);
        }


        void Cut()
        {
            cameraOffset.z = -distance;

            cameraTransform.position = transform.position + cameraOffset;

            cameraTransform.eulerAngles = Vector3.zero;
        }
    }
}
