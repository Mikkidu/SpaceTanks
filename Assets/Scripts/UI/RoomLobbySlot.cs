using UnityEngine;
using TMPro;

namespace AlexDev.SpaceTanks
{
    public class RoomLobbySlot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _roomNameText;
        [SerializeField] private TextMeshProUGUI _playersCountText;

        private string _roomName;

        public string GetRoomName => _roomName;

        public void SetStats(string name, int playersCount)
        {
            _roomNameText.text = name;
            _playersCountText.text = playersCount.ToString();
            _roomName = name;
        }

        public void RefreshStats(int playersCount)
        {
            _playersCountText.text = playersCount.ToString();
        }

        public void RemoveRoom()
        {
            Destroy(gameObject);
        }
    }
}
