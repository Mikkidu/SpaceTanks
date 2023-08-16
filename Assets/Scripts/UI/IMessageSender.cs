using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexDev.SpaceTanks
{
    public interface IMessageSender
    {
        delegate void OnMessageSend(string message);
        public OnMessageSend OnMessageSendEvent { get; set; }
    }
}
