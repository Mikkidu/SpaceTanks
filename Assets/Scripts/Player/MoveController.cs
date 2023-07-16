using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

namespace AlexDev.SpaceTanks
{
    [RequireComponent(typeof(UnitHealth))]
    public class MoveController : MonoBehaviourPun
    {
        [SerializeField] private float _rotateSmooth;
        [SerializeField] private float _moveSpeed;

        private float _rotateSpeed;
        private UnitHealth _shipHealth;


#if PLATFORM_ANDROID
        private Joystick _joystick;
        public Joystick SetJoystick
        {
            set { _joystick = value; }
        }
#endif

        private void Awake()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected)
            {
                enabled = false;
                return;
            }
        }

        void Start()
        {
            CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();
            _shipHealth = GetComponent<UnitHealth>();
            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }
        }

        void Update()
        {
            if (_shipHealth.IsAlive)
            {
#if PLATFORM_ANDROID
                Move(_joystick.Direction);
#else
                Move(GetKeybordDirection());
#endif
            }
        }

        private void Move(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                float directionAngle = Vector2.SignedAngle(transform.up, direction) + transform.eulerAngles.z;
                float newAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, directionAngle, ref _rotateSpeed, _rotateSmooth * Time.deltaTime);

                transform.eulerAngles = Vector3.forward * (newAngle % 360);
                transform.Translate(transform.up * _moveSpeed * Time.deltaTime, Space.World);
            }
        }

        private Vector2 GetKeybordDirection()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticallInput = Input.GetAxis("Vertical");
            return new Vector2(horizontalInput, verticallInput);
        }
    }
}
