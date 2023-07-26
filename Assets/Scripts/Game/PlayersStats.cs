using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

namespace AlexDev.SpaceTanks
{
    public class PlayersStats : MonoBehaviourPunCallbacks
    {
        public static PlayersStats Instance;
        [SerializeField] private List<PlayerGameData> _playersList = new List<PlayerGameData>();
        [SerializeField] private TextMeshProUGUI _leaderText;

        public delegate void OnPlayerListChanged(string userID, UpdateStates updateState);
        [PunRPC] public OnPlayerListChanged onPlayerListChangedEvent;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            
        }

        [PunRPC]
        private void AddPlayer(Player newPlayer, int viewID)
        {
            PlayerGameData player = new PlayerGameData 
            { 
                Name = newPlayer.NickName, 
                UserID = newPlayer.UserId,
                ViewID = viewID
            };

            _playersList.Add(player);
            if (onPlayerListChangedEvent != null)
                onPlayerListChangedEvent.Invoke(player.UserID, UpdateStates.Add);
            if (_playersList != null && _playersList.Count > 0)
            {
                foreach (PlayerGameData playersStats in _playersList)
                {
                    Debug.Log(playersStats);
                }
            }
            UpdatePanel();
        }

        [PunRPC]
        public void AddPlayerCoins(int playerViewID, int coinsAmount)
        {
            PlayerGameData updPlayer = _playersList.Find(player => player.ViewID == playerViewID);
            if (updPlayer != null)
            {
                updPlayer.Coins += coinsAmount;
                photonView.RPC("UpdatePlayerCoins", RpcTarget.All, playerViewID, updPlayer.Coins);
            }
            else
            {
                Debug.LogError("<Color=Red>Missing</Color>" + playerViewID + " " + name);
            }
        }

        [PunRPC]
        private void UpdatePlayerCoins(int playerViewID, int coinsAmount)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                PlayerGameData playerSlot = _playersList.Find(player => player.ViewID == playerViewID);
                playerSlot.Coins = coinsAmount;
            }
            UpdatePanel();
        }

        [PunRPC]
        public void AddPlayerFrag(int playerViewID)
        {
            PlayerGameData updPlayer = _playersList.Find(player => player.ViewID == playerViewID);
            if (updPlayer != null)
            {
                updPlayer.Frags++;
                photonView.RPC("UpdatePlayerFrags", RpcTarget.All, playerViewID, updPlayer.Frags);
            }
            else
            {
                Debug.Log(playerViewID + " " + name);
            }
        }

        [PunRPC]
        private void UpdatePlayerFrags(int playerViewID, int frags)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                PlayerGameData playerSlot = _playersList.Find(player => player.ViewID == playerViewID);
                playerSlot.Frags = frags;
            }
            UpdatePanel();
        }

        private void RemovePlayer(string userID)
        {
            _playersList.Remove(_playersList.Find(player => player.UserID == userID));
            if (onPlayerListChangedEvent != null)
                onPlayerListChangedEvent.Invoke(userID, UpdateStates.Remove);
            UpdatePanel();
        }

        public PlayerGameData GetPlayer(string userID)
        {
            return _playersList.Find(player => player.UserID == userID);
        }


        private void UpdatePanel()
        {
            string color = string.Empty;
            string outputText = string.Empty;
            foreach(PlayerGameData player in _playersList)
            {
                color = "blue";
                outputText += $"{player.Name, 10} {player.Frags, 4} <color={color}>{player.Coins, 4}</color>\n";
            }
            _leaderText.SetText(outputText);
        }
    }
}
