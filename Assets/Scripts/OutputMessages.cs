using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AlexDev.SpaceTanks
{
    public class OutputMessages : MonoBehaviour
    {
        [Tooltip("State output text")]
        [SerializeField]
        private TextMeshProUGUI _stateOutputText;
        [SerializeField] private GameObject[] _messagesSenders;

        private void Start()
        {
            //_messagesSender.OnMasterConnectionChangeEvent += ConnectingOutput;
            _stateOutputText.text = "Connecting to server";
            foreach(GameObject sender in _messagesSenders)
            {
                if (sender.TryGetComponent<IMessageSender>(out IMessageSender messageSender))
                    messageSender.OnMessageSendEvent += OutputMessage;
            }
        }

        public void ConnectingOutput(bool isConnected)
        {
            if (isConnected)
            {
                OutputMessage("Connected to server", Color.green);
            }
            else
            {
                OutputMessage("Disconnected from server", Color.red);
            }
        }
        
        public void OutputMessage(string message)
        {
            OutputMessage(message, Color.white);
        }

        public void OutputMessage(string message, Color color)
        {
            if (_stateOutputText != null)
            {
                _stateOutputText.text = message;
                _stateOutputText.color = color;
            }
            else
            {
                Debug.Log("Output not connected");
            }
        }
    }
}
