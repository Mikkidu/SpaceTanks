using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

namespace AlexDev.SpaceTanks
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        [Tooltip("The maximum number of players per room.")]
        [SerializeField]
        private int _maxPlayersPerRoom = 4;
        
        [Tooltip("Rooms table.")]
        [SerializeField]
        private RoomsLobbyTable _roomsTable;

        [Tooltip("Create room panel")]
        [SerializeField]
        private InputPanelUI _createRoomPanel;
        
        [Tooltip("Join room panel")]
        [SerializeField]
        private InputPanelUI _joinRoomPanel;

        [Tooltip("Player name panel")]
        [SerializeField]
        private InputPanelUI _playerNamePanel;

        [Tooltip("Close room window")]
        [SerializeField]
        private GameObject _closeRoomPanel;

        [Tooltip("Reconnect Button")]
        [SerializeField]
        private GameObject _reconnectButton;

        private bool _isConnectedToMaster;
        private bool _isRedyToEnter;
        private bool _isRoomChange;

        private string _lastRoomName;

        public bool IsConnectedToMaster
        {
            get { return _isConnectedToMaster; }
            set
            {
                _isConnectedToMaster = value;
                if (_reconnectButton != null)
                    _reconnectButton.SetActive(!_isConnectedToMaster);
                if (OnMasterConnectionChangeEvent != null)
                    OnMasterConnectionChangeEvent.Invoke(_isConnectedToMaster);
            }
        }

        public delegate void OnMasterConnectionChange(bool isConnected);
        public OnMasterConnectionChange OnMasterConnectionChangeEvent;

        public delegate void OnMessageSend(string message);
        public OnMessageSend OnMessageSendEvent;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            _createRoomPanel.OnNameAcceptedEvent += CreateRoom;
            _joinRoomPanel.OnNameAcceptedEvent += JoinRoom;
            _playerNamePanel.OnNameAcceptedEvent += ChangeName;
            OnMasterConnectionChangeEvent += _createRoomPanel.OnReadyStateChange;
            OnMasterConnectionChangeEvent += _joinRoomPanel.OnReadyStateChange;
        }

        private void Start()
        {
            if (!PhotonNetwork.IsConnected)
                Connect();
            if (PlayerPrefs.HasKey(Constants.PLAYER_NAME_PREF_KEY))
                PhotonNetwork.NickName = PlayerPrefs.GetString(Constants.PLAYER_NAME_PREF_KEY);
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            IsConnectedToMaster = true;
            PhotonNetwork.JoinLobby();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            IsConnectedToMaster = false;
        }


        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRoomFailed() was called by PUN. No random room available or room is full.");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            if (OnMessageSendEvent != null)
                OnMessageSendEvent.Invoke("Connected to room. Ready for players");
            /*if (_isRedyToEnter && PhotonNetwork.CurrentRoom.PlayerCount > 1)
                LoadLevel();*/
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            if (_isRedyToEnter && PhotonNetwork.CurrentRoom.PlayerCount > 1)
                LoadLevel();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("PUN Launcher: OnJoinedToLobby() was called by PUN");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("PUN Launcher: OnRoomListUpdate() was called by PUN");
            _roomsTable.RefreshRoomList(roomList);            
        }

        public void Connect()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public void CreateRoom(string roomName)
        {
            if (_isConnectedToMaster)
            {
                PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = _maxPlayersPerRoom, IsOpen = true, PublishUserId = true });
                _lastRoomName = roomName;
            }
        }

        public void JoinRoom(string roomName)
        {
            if (!PhotonNetwork.InRoom)
                PhotonNetwork.JoinRoom(roomName);
            else if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
                LoadLevel();
            _isRedyToEnter = true;
            /*if (!PhotonNetwork.InRoom)
            {
                if (roomName != PhotonNetwork.CurrentRoom.Name)
                {
                    _lastRoomName = roomName;
                    string message = "You are alredy connectod to another room.";
                    Debug.Log(message);
                    _closeRoomPanel.SetActive(true);
                    if (OnMessageSendEvent != null)
                        OnMessageSendEvent.Invoke(message);
                }
                PhotonNetwork.JoinRoom(roomName);
            }
            else if (roomName != PhotonNetwork.CurrentRoom.Name)
            {
                string message = "We load the room";
                Debug.Log(message);
                if (OnMessageSendEvent != null)
                    OnMessageSendEvent.Invoke(message);
            }*/
        }
        

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void ChangeName(string name)
        {
            PhotonNetwork.NickName = name;
        }

        public void LoadLevel()
        {
            if (_isRedyToEnter && PhotonNetwork.CurrentRoom.PlayerCount > 0)
            {
                string message = "We load the room";
                Debug.Log(message);
                if (OnMessageSendEvent != null)
                    OnMessageSendEvent.Invoke(message);
                PhotonNetwork.LoadLevel("Game");
            }
        }
    }
}
