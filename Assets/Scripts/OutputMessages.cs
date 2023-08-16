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
        [SerializeField] private static List<IMessageSender> _messagesSenders = new List<IMessageSender>();

        private void Start()
        {
            //_messagesSender.OnMasterConnectionChangeEvent += ConnectingOutput;
            _stateOutputText.text = "Console";
            foreach(IMessageSender sender in _messagesSenders)
            {
                if (sender != null)
                    sender.OnMessageSendEvent += OutputMessage;
            }
        }

        public static void AddMessgeSender(IMessageSender messageSender)
        {
            if (!_messagesSenders.Contains(messageSender))
            _messagesSenders.Add(messageSender);
        }

        public static void RemoveMessageSender(IMessageSender messageSender)
        {
            _messagesSenders.Remove(messageSender);
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
