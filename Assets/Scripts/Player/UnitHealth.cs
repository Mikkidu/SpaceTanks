using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace AlexDev.SpaceTanks
{
    public class UnitHealth : MonoBehaviourPunCallbacks
    {
        [SerializeField] private int _health;
        [SerializeField] private int _maxHealth;
        [SerializeField] private PlayerUI _playerUIPrefab;

        private bool isDead;

        public delegate void OnHealthChanged(int hp);
        public OnHealthChanged OnHealthChangedEvent;

        public bool IsAlive => !isDead;
        public int GetMaxHealth => _maxHealth;

        private void Awake()
        {
            _health = _maxHealth;
        }

        private void Start()
        {
            if (OnHealthChangedEvent != null)
                OnHealthChangedEvent.Invoke(_health);
            if (_playerUIPrefab != null)
            {
                Instantiate(_playerUIPrefab).SetTarget(this);
            }
            else
            {
                Debug.LogWarning("<Color=Yellow><a>Missing</a></Color> PlayerUI component on Player prefab.", this);
            }
        }

        public bool TakeDamage(int damage, int viewId)
        {
            if (viewId != photonView.ViewID)
            {
                ChangeHealth(-damage);
                if (!PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("ChangeHealth", RpcTarget.MasterClient, -damage);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("UpdateHealth", RpcTarget.Others, _health);
            }
        }

        [PunRPC]
        private void ChangeHealth(int damage)
        {
            _health += damage;
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("UpdateHealth", RpcTarget.All, _health);
            }
            else
            {
                if (OnHealthChangedEvent != null) OnHealthChangedEvent.Invoke(_health);
            }
        }

        [PunRPC]
        private void UpdateHealth(int newHealth)
        {
            if (!PhotonNetwork.IsMasterClient)
                _health = newHealth;
            if (_health <= 0)
                Death();
            if (OnHealthChangedEvent != null) OnHealthChangedEvent.Invoke(_health);
        }

        protected virtual void Death()
        {
            if (IsAlive)
            {
                if (photonView.IsMine)
                    GameManager.Instance.LeaveRoom();

                isDead = true;
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }

}
