using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace AlexDev.SpaceTanks
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance;

        [SerializeField] private GameObject _playerPrefab;

        public delegate void OnCoinsChange(int coinsValue);
        public static OnCoinsChange OnCoinsChangeEvent;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Color"))
            {
                InstancePlayer();
            }
            else
            {
                StartCoroutine("WaitingForPlayerStats");
            }
        }

        private IEnumerator WaitingForPlayerStats()
        {
            bool isStatsEmpty = true;
            while (isStatsEmpty)
            {
                if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Color"))
                {
                    isStatsEmpty = false;
                }
                else
                {
                    yield return new WaitForSeconds(0.5f);
                }
            }
            InstancePlayer();
        }

        public void InstancePlayer()
        {
            if (_playerPrefab == null)
            {
                Debug.LogError("<Color=red><a>Missing</a></Color> playerPrefab Reference.");
            }
            else
            {
                Debug.Log($"We are Instatiating LocalPlayer from {SceneManager.GetActiveScene().name}");
                GameObject player = PhotonNetwork.Instantiate(
                    _playerPrefab.name,
                    new Vector2(
                        Random.Range(-3, 3), 
                        Random.Range(-3, 3)),
                    Quaternion.identity);
            }
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Lobby");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0} {1}", newPlayer.NickName, newPlayer.CustomProperties["Color"]);
        }

        public override void OnPlayerLeftRoom(Player player)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", player.NickName);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}
