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
        [SerializeField] private GameUI _gameUI;

        private int _deadPlayersCount = 0;
        private bool _isGameOn = false;

        private bool CanStartGame
        {
            get
            {
                return PhotonNetwork.PlayerList.Length > 1;
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            PlayersStatsManager.Instance.OnPlayerDieEvent += OnPlayerDie;
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Color"))
            {
                InstancePlayer();
            }
            else
            {
                StartCoroutine("WaitingForPlayerStats");
            }
            if (CanStartGame)
                StartGame();
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
                    yield return new WaitForSeconds(0.1f);
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

        private void StartGame()
        {
            _isGameOn = true;
        }

        private void OnPlayerDie(int viewID)
        {
            Debug.Log("PlayerDie");
            if (PlayersStatsManager.Instance.LivePlayersCount <= 1)
            {
                Debug.Log("PlayerDie");
                _isGameOn = false;
                _gameUI.ShowWinPanel();
            }
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Lobby");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0} {1}", newPlayer.NickName, newPlayer.CustomProperties["Color"]);
            if (!_isGameOn && CanStartGame)
                StartGame();
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
