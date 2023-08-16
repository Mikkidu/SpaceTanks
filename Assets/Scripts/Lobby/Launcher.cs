using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


namespace AlexDev.SpaceTanks
{
    public class Launcher : MonoBehaviourPunCallbacks, IMessageSender
    {
        public static Launcher Instance;

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

        private bool _isRedyToEnter;

        private IMessageSender.OnMessageSend _onMessageSendEvent;
        public IMessageSender.OnMessageSend OnMessageSendEvent
        {
            get { return _onMessageSendEvent; }
            set { _onMessageSendEvent = value; }
        }

        private void Awake()
        {
            Instance = this;
 
            PhotonNetwork.AutomaticallySyncScene = true;
            _createRoomPanel.OnNameAcceptedEvent += CreateRoom;
            _joinRoomPanel.OnNameAcceptedEvent += JoinRoom;
            _playerNamePanel.OnNameAcceptedEvent += ChangeName;
        }

        private void Start()
        {
            AudioManager.instance.PlayMusicIfDifferent("MenuMusic");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRoomFailed() was called by PUN. No random room available or room is full.");
            if (OnMessageSendEvent != null)
                OnMessageSendEvent.Invoke("Connected to room <color=red>failed</color>.");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            if (OnMessageSendEvent != null)
                OnMessageSendEvent.Invoke("Connected to room. Ready for players");
            LoadLevel();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            if (_isRedyToEnter && PhotonNetwork.CurrentRoom.PlayerCount > 1)
                LoadLevel();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("PUN Launcher: OnRoomListUpdate() was called by PUN");
            _roomsTable.RefreshRoomList(roomList);            
        }

        public void CreateRoom(string roomName)
        {
            if (ConnectionManager.Instance.IsConnectedToMaster)
            {
                PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = _maxPlayersPerRoom, IsOpen = true, PublishUserId = true });
            }
        }

        public void JoinRoom(string roomName)
        {
            _isRedyToEnter = true;
            if (!PhotonNetwork.InRoom)
                PhotonNetwork.JoinRoom(roomName);
            else
                LoadLevel();
        }
        

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void ChangeName(string name)
        {
            PhotonNetwork.NickName = name;
            DataManager.instance.SaveName(name);
        }

        public void LoadLevel()
        {
            //if (!_isRedyToEnter)
            //    return;

            if (ScenesStateMachine.ChangeScene(ScenesStates.Game))
            {
                string message = "We load the room";
                Debug.Log(message);
                if (OnMessageSendEvent != null)
                    OnMessageSendEvent.Invoke(message);
            }
        }
    }
}
