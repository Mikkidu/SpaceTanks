using System.Collections;
using System;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace AlexDev.SpaceTanks
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance;

        [SerializeField] private GameObject _playerPrefab;
#if PLATFORM_ANDROID
        [SerializeField] private JoystickManager _joystickManagerPrefab;
#endif

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (_playerPrefab == null)
            {
                Debug.LogError("<Color=red><a>Missing</a></Color> playerPrefab Reference.");
            }
            else
            {
                Debug.Log($"We are Instatiating LocalPlayer from {SceneManager.GetActiveScene().name}");
                InstantPlayer();
            }
        }

        private void InstantPlayer()
        {
            GameObject player = PhotonNetwork.Instantiate(
                    _playerPrefab.name,
                    new Vector2(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3)),
                    Quaternion.identity);
#if PLATFORM_ANDROID
            AddJoystick(player);
#endif
        }

#if PLATFORM_ANDROID
        private void AddJoystick(GameObject player)
        {
            JoystickManager joystickManager = Instantiate(_joystickManagerPrefab, GameObject.FindObjectOfType<Canvas>().transform);
            player.GetComponent<MoveController>().SetJoystick = joystickManager.GetLeftJoystick;
            player.GetComponent<GunController>().SetJoystick = joystickManager.GetRightJoystick;
        }
#endif

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Lobby");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName);
        }

        public override void OnPlayerLeftRoom(Player player)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", player.NickName);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

    }
}
