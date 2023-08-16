using UnityEngine;

namespace AlexDev.SpaceTanks
{
    public class ReconnectButton : MonoBehaviour
    {
        private void Start()
        {
            ConnectionManager.Instance.SetReconnectButton = gameObject;
            gameObject.SetActive(false);
        }
    }
}
