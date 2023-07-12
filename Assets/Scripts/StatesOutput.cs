using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AlexDev.SpaceTanks
{
    public class StatesOutput : MonoBehaviour
    {
        [Tooltip("State output text")]
        [SerializeField]
        private TextMeshProUGUI _stateOutputText;

        [SerializeField] private Launcher _launcher;

        private void Start()
        {
            _launcher.OnMasterConnectionChangeEvent += ConnectingOutput;
            _stateOutputText.text = "Connecting to server";
            _launcher.OnMessageSendEvent += OutputMessage;
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
