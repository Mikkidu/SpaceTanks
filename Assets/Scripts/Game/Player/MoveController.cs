using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Photon.Pun;

namespace AlexDev.SpaceTanks
{
    [RequireComponent(typeof(PlayerHealth))]
    public class MoveController : MonoBehaviourPun
    {
        [SerializeField] private float _rotateSmooth;
        [SerializeField] private float _moveSpeed;
#if PLATFORM_ANDROID
        [SerializeField] private JoystickManager _joystickManagerPrefab;
#endif

        private float _rotateSpeed;
        private PlayerHealth _shipHealth;


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
#if PLATFORM_ANDROID
            AddJoystick();
#endif
        }

#if PLATFORM_ANDROID
        private void AddJoystick()
        {
            JoystickManager joystickManager = Instantiate(_joystickManagerPrefab, GameObject.FindObjectOfType<Canvas>().transform);
            GetComponent<MoveController>().SetJoystick = joystickManager.GetLeftJoystick;
            GetComponent<GunController>().SetJoystick = joystickManager.GetRightJoystick;
            _rotateSmooth /= 2;
        }
#endif

        void Start()
        {
            CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();
            _shipHealth = GetComponent<PlayerHealth>();
            PlayersStatsManager.Instance.MyViewID = photonView.ViewID;
            /*if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }*/
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
            if (!GameManager.Instance.IsGameOn)
                return;
            if (direction != Vector2.zero)
            {
                float directionAngle = Vector2.SignedAngle(transform.up, direction) + transform.eulerAngles.z;
                float newAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, directionAngle, ref _rotateSpeed, _rotateSmooth, Mathf.Infinity, Time.deltaTime) % 360;

                transform.eulerAngles = Vector3.forward * newAngle;
                transform.Translate(transform.up * _moveSpeed * Time.deltaTime, Space.World);
            }
        }

#if !PLATFORM_ANDROID
        private Vector2 GetKeybordDirection()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticallInput = Input.GetAxis("Vertical");
            return new Vector2(horizontalInput, verticallInput);
        }
#endif
    }
}
