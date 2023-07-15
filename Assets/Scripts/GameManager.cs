using System.Collections;
using System;

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

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (_playerPrefab == null)
            {
                Debug.LogError("<Color=red><a>Missing</a></Color> playerPrefab Reference.");
            }
            else
            {
                Debug.Log($"We are Instatiating LocalPlayer from {SceneManager.GetActiveScene().name}");
                PhotonNetwork.Instantiate(
                    _playerPrefab.name,
                    new Vector2(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3)),
                    Quaternion.identity);

            }
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Lobby");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName);
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
