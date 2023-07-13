using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexDev.SpaceTanks
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _rotateSmooth;
        [SerializeField] private float _moveSpeed;

        private float _rotateSpeed;

        void Start()
        {
            
        }

        void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticallInput = Input.GetAxis("Vertical");
            Vector2 direction = new Vector2(horizontalInput, verticallInput);
            if (direction != Vector2.zero)
            {
                float directionAngle = Vector2.SignedAngle(transform.up, direction) + transform.eulerAngles.z;
                float newAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, directionAngle, ref _rotateSpeed, _rotateSmooth);

                transform.eulerAngles = Vector3.forward * (newAngle % 360);
                transform.Translate(transform.up * _moveSpeed / 50, Space.World);
            }

        }
    }
}
