using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace AlexDev.SpaceTanks
{
    public class PlayerHealth : MonoBehaviourPunCallbacks
    {
        [SerializeField] private int _health;
        [SerializeField] private int _maxHealth;
        [SerializeField] private PlayerUI _playerUIPrefab;

        private bool isDead;
        private int _lastHitPersonViewID;

        public delegate void OnHealthChanged(int hp);
        public OnHealthChanged OnHealthChangedEvent;

        public bool IsAlive => !isDead;
        public int GetMaxHealth => _maxHealth;

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
                UpdateForNewPlayer(newPlayer);
        }

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

        public bool TakeDamage(int damage, int attackerViewID)
        {
            if (attackerViewID != photonView.ViewID)
            {
                _lastHitPersonViewID = attackerViewID;
                ChangeHealth(-damage, attackerViewID);
                if (!PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("ChangeHealth", RpcTarget.MasterClient, -damage, attackerViewID);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateForNewPlayer(Player player)
        {
                photonView.RPC("UpdateHealth", player, _health, _lastHitPersonViewID);
        }

        [PunRPC]
        private void ChangeHealth(int damage, int attackerViewID)
        {
            _health += damage;
            if (attackerViewID != 0)
                _lastHitPersonViewID = attackerViewID;
            Debug.Log("hit from" + _lastHitPersonViewID);
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("UpdateHealth", RpcTarget.All, _health, _lastHitPersonViewID);
            }
            else
            {
                if (OnHealthChangedEvent != null) OnHealthChangedEvent.Invoke(_health);
            }
        }

        [PunRPC]
        private void UpdateHealth(int newHealth, int attackerViewID)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                _health = newHealth;
                _lastHitPersonViewID = attackerViewID;
            }
            if (_health <= 0)
                Death();
            if (OnHealthChangedEvent != null) OnHealthChangedEvent.Invoke(_health);
        }

        protected virtual void Death()
        {
            if (IsAlive)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PlayersStatsManager.Instance.AddPlayerFrag(_lastHitPersonViewID, photonView.ViewID);
                    Debug.Log($"Player <Color=Red>die</Red> T.T" + name);
                }
                isDead = true;
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }

}
