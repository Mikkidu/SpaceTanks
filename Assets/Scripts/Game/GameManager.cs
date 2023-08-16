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
        [SerializeField] private Transform _bottomLeftCorner;
        [SerializeField] private Transform _upperRightCorner;

        private bool _isGameOn = false;
        public bool IsGameOn => _isGameOn;

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
                float randomX = Random.Range(_bottomLeftCorner.position.x, _upperRightCorner.position.x);
                float randomY = Random.Range(_bottomLeftCorner.position.y, _upperRightCorner.position.y);
                PhotonNetwork.Instantiate(_playerPrefab.name, new Vector2(randomX, randomY), Quaternion.identity);
            }
        }

        private void StartGame()
        {
            _isGameOn = true;
            AudioManager.instance.PlayMusic("BattleMusic");
            _gameUI.StartGame();
        }

        private void OnPlayerDie(int viewID)
        {
            if (PlayersStatsManager.Instance.LivePlayersCount <= 1)
            {
                _isGameOn = false;
                _gameUI.ShowWinPanel();
            }
        }

        public override void OnLeftRoom()
        {
            ScenesStateMachine.ChangeScene(ScenesStates.Lobby);
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
