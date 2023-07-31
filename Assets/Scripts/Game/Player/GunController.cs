using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

namespace AlexDev.SpaceTanks
{
    [RequireComponent(typeof(PlayerHealth))]
    public class GunController : MonoBehaviourPun
    {
        [SerializeField] private float _attackInterval = 0.5f;
        [SerializeField] private int _unitDamage;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private Transform _gun;
        [SerializeField] private float _rotationSmoothness;

        private PlayerHealth _shipHealthScript;
        private float _attackTimer;
        private float _rotateSpeed;
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
            _shipHealthScript = GetComponent<PlayerHealth>();
        }

        void LateUpdate()
        {
            if (!GameManager.Instance.IsGameOn)
                return;
            if (_shipHealthScript.IsAlive)
            {
                Aiming();
            }
        }

        private void Aiming()
        {
#if PLATFORM_ANDROID
            if (_joystick.Direction != Vector2.zero)
            {
                GunRotating(_joystick.Direction);
#else
            if (Input.GetMouseButton(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GunRotating(mousePosition - (Vector2)transform.position);
#endif
                Attack();
            }
            else
            {
                Rest();
            }
        }

        private void Rest()
        {
            GunRotating(transform.up);
        }

        private void GunRotating(Vector2 direction)
        {
            float directionAngle = Vector2.SignedAngle(_gun.up, direction) + _gun.eulerAngles.z;
            float newAngle = Mathf.SmoothDamp(_gun.eulerAngles.z, directionAngle, ref _rotateSpeed, _rotationSmoothness, Mathf.Infinity, Time.deltaTime) % 360;
            _gun.eulerAngles = Vector3.forward * newAngle;
        }

        private void Attack()
        {
            if (_attackTimer <= Time.realtimeSinceStartup)
            {
                PhotonNetwork.Instantiate(_projectilePrefab.name, _shootPoint.position, _shootPoint.rotation)
                    .GetComponent<Projectile>()
                    .photonView.RPC("Initialization", RpcTarget.All, _unitDamage, photonView.ViewID);
                _attackTimer = Time.realtimeSinceStartup + _attackInterval;
            }
        }

    }
}
