using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

namespace AlexDev.SpaceTanks
{
    [RequireComponent(typeof(UnitHealth))]
    public class GunController : MonoBehaviourPun
    {
        [SerializeField] private float _attackInterval = 0.5f;
        [SerializeField] private int _unitDamage;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private Transform _gun;
        [SerializeField] private float _rotationSmoothness;

        private UnitHealth _shipHealthScript;
        private float _attackTimer;
        private float _rotateSpeed;


        private void Awake()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected)
            {
                enabled = false;
                return;
            }
            _shipHealthScript = GetComponent<UnitHealth>();
        }
        void Start()
        {
        
        }

        void LateUpdate()
        {
            if (_shipHealthScript.IsAlive)
            {
                if (Input.GetMouseButton(0))
                {
                    Aiming();
                    Attack();
                }
                else
                {
                    Rest();
                }
            }
        }

        private void Rest()
        {
            GunRotating(transform.up);
        }

        private void Aiming()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GunRotating(mousePosition - (Vector2)transform.position);
        }

        private void GunRotating(Vector2 direction)
        {
            float directionAngle = Vector2.SignedAngle(_gun.up, direction) + _gun.eulerAngles.z;
            float newAngle = Mathf.SmoothDamp(_gun.eulerAngles.z, directionAngle, ref _rotateSpeed, _rotationSmoothness) % 360;
            _gun.eulerAngles = Vector3.forward * newAngle;
        }

        private void Attack()
        {
            if (_attackTimer <= Time.realtimeSinceStartup)
            {
                Debug.Log("Bam!");
                PhotonNetwork.Instantiate(_projectilePrefab.name, _shootPoint.position, _shootPoint.rotation)
                    .GetComponent<Projectile>()
                    .Initialization(_unitDamage, photonView.ViewID);
                _attackTimer = Time.realtimeSinceStartup + _attackInterval;
            }
        }

    }
}
