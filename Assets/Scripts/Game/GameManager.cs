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

        private static int _coinsValue;

        public delegate void OnCoinsChange(int coinsValue);
        public static OnCoinsChange OnCoinsChangeEvent;


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
                InstantPlayer();
            }
        }

        private void InstantPlayer()
        {
            GameObject player = PhotonNetwork.Instantiate(
                _playerPrefab.name,
                new Vector2(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3)),
                Quaternion.identity);
            PlayersStats.Instance.photonView.RPC(
                "AddPlayer", 
                RpcTarget.All, 
                PhotonNetwork.LocalPlayer, 
                player.GetComponent<PhotonView>().ViewID);
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

        public static void AddCoins(int amount)
        {
            _coinsValue += amount;
            if (OnCoinsChangeEvent != null)
                OnCoinsChangeEvent.Invoke(_coinsValue);
        }

        [PunRPC]
        private static void UpdateCoins(int coinsAmount)
        {

        }

        public static void SpendCoins(int price)
        {

        }
    }
}
