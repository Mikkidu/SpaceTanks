using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace AlexDev.SpaceTanks
{
    public class ConnectionManager : MonoBehaviourPunCallbacks, IMessageSender
    {
        public static ConnectionManager Instance;

        [Tooltip("Reconnect Button")]
        [SerializeField]
        private GameObject _reconnectButton;

        private bool _isConnectedToMaster = false;

        private IMessageSender.OnMessageSend _onMessageSendEvent;
        public IMessageSender.OnMessageSend OnMessageSendEvent
        {
            get { return _onMessageSendEvent; }
            set { _onMessageSendEvent = value; }
        }

        public delegate void OnMasterConnectionChange(bool isConnected);
        public OnMasterConnectionChange OnMasterConnectionChangeEvent;

        public GameObject SetReconnectButton
        {
            set { _reconnectButton = value; }
        }

        public bool IsConnectedToMaster
        {
            get { return _isConnectedToMaster; }
            set
            {
                if (value == _isConnectedToMaster)
                    return;
                if (OnMasterConnectionChangeEvent != null)
                    OnMasterConnectionChangeEvent.Invoke(value);
                _isConnectedToMaster = value;
                if (_reconnectButton != null & _reconnectButton.activeSelf == _isConnectedToMaster)
                    _reconnectButton.SetActive(!_isConnectedToMaster);
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
            OutputMessages.AddMessgeSender(this);
        }

        private void OnDestroy()
        {
            OutputMessages.RemoveMessageSender(this);
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
