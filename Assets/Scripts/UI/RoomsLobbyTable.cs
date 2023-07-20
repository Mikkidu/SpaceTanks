using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;
namespace AlexDev.SpaceTanks
{
    public class RoomsLobbyTable : MonoBehaviour
    {
        [SerializeField] private Transform _roomsTable;
        [SerializeField] private RoomLobbySlot _roomSlotPrefab;

        private List<RoomLobbySlot> _roomsList = new List<RoomLobbySlot>();

        public bool IsEmpty
        {
            get { return _roomsList.Count == 0; }
        }

        public void RefreshRoomList(List<RoomInfo> roomList)
        {
            
            if (IsEmpty)
            {
                foreach (RoomInfo room in roomList)
                {
                    if (!room.RemovedFromList)
                        AddRoom(room);
                }
            }
            else
            {
                foreach (RoomInfo room in roomList)
                {
                    Debug.Log(room.Name + " " + room.RemovedFromList);
                    if (_roomsList.Exists(slot => slot.GetRoomName == room.Name))
                    {
                        if (!room.RemovedFromList)
                        {
                            RefreshRoom(room);
                        }
                        else
                        {
                            RemoveRoom(room);
                        }
                    }
                    else
                    {
                        AddRoom(room);
                    }
                }
            }
        }

        private void AddRoom(RoomInfo roomInfo)
        {
            RoomLobbySlot newRoom = Instantiate(_roomSlotPrefab, _roomsTable.transform);
            newRoom.SetStats(roomInfo.Name, roomInfo.PlayerCount);
            _roomsList.Add(newRoom);
        }

        private void RefreshRoom(RoomInfo roomInfo)
        {
            _roomsList.Find(room => room.GetRoomName == roomInfo.Name).RefreshStats(roomInfo.PlayerCount);
        }

        private void RemoveRoom(RoomInfo roomInfo)
        {
            RoomLobbySlot room = _roomsList.Find(slot => slot.GetRoomName == roomInfo.Name);
            room.RemoveRoom();
            _roomsList.Remove(room);
        }
    }
}
