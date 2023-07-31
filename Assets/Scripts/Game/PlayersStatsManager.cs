using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace AlexDev.SpaceTanks
{
    public class PlayersStatsManager : MonoBehaviourPunCallbacks
    {
        public static PlayersStatsManager Instance;

        private int _myViewID;

        public int MyViewID
        {
            get { return _myViewID; }
            set { _myViewID = value; }
        }

        public int LivePlayersCount
        {
            get
            {
                int count= 0;
                Player[] players = PhotonNetwork.PlayerList;
                for (int i = 0; i < players.Length; i++)
                {
                    if ((bool)players[i].CustomProperties["IsDead"] == false)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public delegate void OnPlayerDie(int viewID);
        public OnPlayerDie OnPlayerDieEvent;

        public delegate void OnPlayerStateChanged();
        public OnPlayerStateChanged OnPlayerStateChangedEvent;

        public delegate void OnCoinsChange(int coinsValue);
        public OnCoinsChange OnCoinsChangeEvent;


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
        }

        public static void InitializePlayerCustomProperties(Player player)
        {
            Hashtable hash = new Hashtable()
            {
                { "ViewID", 0 },
                { "Color", PlayerColors.GetNewColor },
                { "Frags", 0 },
                { "Coins", 0 },
                { "IsDead", false }
            };
            player.SetCustomProperties(hash);
            Debug.Log(player.NickName + " " + player.CustomProperties["Color"]);
        }

        public static void SetPlayerViewID(int viewID)
        {
            Hashtable hash = new Hashtable { { "ViewID", viewID } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

        public void AddCoins(int playerViewID, int coinsAmount)
        {
            AddIntPlayerState(playerViewID, "Coins", coinsAmount);
        }

        public void AddPlayerFrag(int killerViewID, int victimViewID)
        {
            AddIntPlayerState(killerViewID, "Frags", 1);
            ChangeBoolPlayerState(victimViewID, "IsDead", true);
        }

        public Player GetPlayer(string userID)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.PlayerList[i].UserId == userID)
                {
                    return PhotonNetwork.PlayerList[i];
                }
            }
            return null;
        }

        public string GetFirsLivePlayerName(bool isColored)
        {
            string txt = string.Empty;
            Player[] players = PhotonNetwork.PlayerList;
            for (int i = 0; i < players.Length; i++)
            {
                if ((bool)players[i].CustomProperties["IsDead"] == false)
                {
                    Player player = players[i];
                    txt = player.NickName;
                    if (isColored)
                        txt = $"<color={GetPlayerHexColor(player.UserId)}>" + txt + "</color>";
                    return txt;
                }
            }
            return txt;
        }

        private void AddIntPlayerState(int viewID, string stateName, int addValue)
        {
            Player player = GetPlayer(viewID);
            if (player != null)
            {
                Hashtable hash = new Hashtable();
                hash.Add(stateName, (int)player.CustomProperties[stateName] + addValue);
                player.SetCustomProperties(hash);
            }
            else
            {
                Debug.Log("<Color=Red>Missing</Color>" + viewID + " " + name);
            }
        }

        private void ChangeBoolPlayerState(int viewID, string stateName, bool isTrue)
        {
            Debug.Log(viewID);
            Player player = GetPlayer(viewID);
            if (player != null)
            {
                Hashtable hash = new Hashtable { { stateName, isTrue } };
                player.SetCustomProperties(hash);
            }
            else
            {
                Debug.Log("<Color=Red>Missing</Color>" + viewID + " " + name);
            }
        }

        public Player GetPlayer(int viewID)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if ((int)PhotonNetwork.PlayerList[i].CustomProperties["ViewID"] == viewID)
                {
                    return PhotonNetwork.PlayerList[i];
                }
            }
            return null;
        }

        public Color GetPlayerColor(int viewID)
        {
            string colorName = GetPlayer(viewID).CustomProperties["Color"].ToString();
            return PlayerColors.GetRgbColor(colorName);
        }
        
        public string GetPlayerHexColor(int viewID)
        {
            string colorName = GetPlayer(viewID).CustomProperties["Color"].ToString();
            return PlayerColors.GetHexColor(colorName);
        }
        
        public Color GetPlayerColor(string userID)
        {
            string colorName = GetPlayer(userID).CustomProperties["Color"].ToString();
            return PlayerColors.GetRgbColor(colorName);
        }
        
        public string GetPlayerHexColor(string userID)
        {
            string colorName = GetPlayer(userID).CustomProperties["Color"].ToString();
            return PlayerColors.GetHexColor(colorName);
        }


        public string GetFullPlayersStates()
        {
            string outputText = string.Empty;
            string tempText;
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                string hexColor = PlayerColors.GetHexColor(player.CustomProperties["Color"].ToString());
                tempText = $"{player.NickName,-10}\t";
                tempText += $"{player.CustomProperties["Frags"],4}\t";
                tempText += $"{player.CustomProperties["Coins"],4}";
                if ((bool)player.CustomProperties["IsDead"])
                    tempText += $"Dead";
                tempText = $"<color={hexColor}>" + tempText + "</color>";
                if ((int)player.CustomProperties["ViewID"] == _myViewID)
                    tempText = "<mark>" + tempText + "</mark>";
                outputText += tempText + "\n";
            }
            return outputText;
        }

        public string GetShortPlayersStates()
        {
            string outputText = string.Empty;
            string tempText;
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                string hexColor = PlayerColors.GetHexColor(player.CustomProperties["Color"].ToString());
                tempText = $"<color={hexColor}>{player.NickName,-10}</color>";
                if ((int)player.CustomProperties["ViewID"] == _myViewID)
                    tempText = "<mark>" + tempText + "</mark>";
                outputText += tempText + "\n";
            }
            return outputText;
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            string txt = targetPlayer.NickName + " ";
            foreach (object property in changedProps.Keys)
            {
                txt += property.ToString();
            }
            if (OnPlayerStateChangedEvent != null)
                OnPlayerStateChangedEvent.Invoke();
            if (targetPlayer.IsLocal && changedProps.ContainsKey("Coins"))
                if (OnCoinsChangeEvent != null)
                    OnCoinsChangeEvent.Invoke((int)targetPlayer.CustomProperties["Coins"]);
            if (changedProps.TryGetValue("IsDead", out object isDead) && (bool)isDead)
                if (OnPlayerDieEvent != null)
                    OnPlayerDieEvent.Invoke((int)targetPlayer.CustomProperties["ViewID"]);
            Debug.Log(txt);
            
        }

        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.IsMasterClient)
                InitializePlayerCustomProperties(PhotonNetwork.LocalPlayer);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            InitializePlayerCustomProperties(newPlayer);
        }
    }
}
