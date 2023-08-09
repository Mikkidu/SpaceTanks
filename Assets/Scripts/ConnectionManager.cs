using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace AlexDev.SpaceTanks
{
    public class ConnectionManager : MonoBehaviourPunCallbacks, IMessageSender
    {
        public static ConnectionManager Instance;

        [Tooltip("Reconnect Button")]
        public GameObject reconnectButton;

        private bool _isConnectedToMaster = false;

        private IMessageSender.OnMessageSend _onMessageSendEvent;
        public IMessageSender.OnMessageSend OnMessageSendEvent
        {
            get { return _onMessageSendEvent; }
            set { _onMessageSendEvent = value; }
        }
        public bool IsConnectedToMaster
        {
            get { return _isConnectedToMaster; }
            set
            {
                _isConnectedToMaster = value;
                if (reconnectButton != null & reconnectButton.activeSelf == _isConnectedToMaster)
                    reconnectButton.SetActive(!_isConnectedToMaster);
                if (OnMessageSendEvent != null)
                {
                    string message;
                    if (_isConnectedToMaster)
                        message = "<color=green>Succsessfully connected to the MasterServer</color>";
                    else
                        message = "<color=red>Server connection error</color>";
                    _onMessageSendEvent.Invoke(message);
                }
            }
        }

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
            if (!PhotonNetwork.IsConnected)
                Connect();
            if (PlayerPrefs.HasKey(Constants.PLAYER_NAME_PREF_KEY))
                PhotonNetwork.NickName = PlayerPrefs.GetString(Constants.PLAYER_NAME_PREF_KEY);
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log(gameObject.name + ": OnConnectedToMaster() was called by PUN");
            IsConnectedToMaster = true;
            PhotonNetwork.JoinLobby();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat(gameObject.name + ": OnDisconnected() was called by PUN with reason {0}", cause);
            IsConnectedToMaster = false;
        }

        public override void OnJoinedLobby()
        {
            string message = gameObject.name + ": OnJoinedToLobby() was called by PUN";
            Debug.Log(message);
            if (OnMessageSendEvent != null)
                OnMessageSendEvent.Invoke(message);
        }

        public void Connect()
        {
            PhotonNetwork.ConnectUsingSettings();
            string message = gameObject.name + ": Connecting to masterServer";
            if (OnMessageSendEvent != null)
                OnMessageSendEvent.Invoke(message);
        }
    }
}
