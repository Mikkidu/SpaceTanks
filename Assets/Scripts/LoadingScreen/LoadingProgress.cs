using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Photon.Pun;

namespace AlexDev.SpaceTanks
{
    public class LoadingProgress : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TextMeshProUGUI _messageToUserText;

        private bool _isReadyToStart = false;
        private Animator _animator;


        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (Input.anyKeyDown && !_isReadyToStart)
            {
                _isReadyToStart = true;
                _animator.SetBool("IsReadyToStart", true);
                UpdateMessageToUser();
            }
        }

        private void Start()
        {
            AudioManager.instance.PlayMusic("MenuMusic");
        }

        public override void OnJoinedLobby()
        {
            _animator.SetBool("IsConnected", true);
            UpdateMessageToUser();
        }

        private void UpdateMessageToUser()
        {
            string message;
            if (_isReadyToStart)
            {
                if (PhotonNetwork.IsConnectedAndReady)
                    message = "To infinity... and beyond!";
                else
                    message = "One moment!";
            }
            else
            {
                if (PhotonNetwork.IsConnectedAndReady)
                    message = _messageToUserText.text + "\nwe are ready!";
                else
                    return;
            }
            Debug.Log(message);
            _messageToUserText.SetText(message);
        }

        private void LoadLobby()
        {
                ScenesStateMachine.ChangeScene(ScenesStates.Lobby);
        }
    }
}
