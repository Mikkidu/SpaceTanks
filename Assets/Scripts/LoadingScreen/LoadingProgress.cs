using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

namespace AlexDev.SpaceTanks
{
    public class LoadingProgress : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject _reconnectButton;

        private bool _isReadyToStart = false;
        private Animator _animator;


        private void Awake()
        {
            _animator = GetComponent<Animator>();
            ConnectionManager.Instance.reconnectButton = _reconnectButton;
        }

        private void Update()
        {
            if (Input.anyKeyDown && !_isReadyToStart)
            {
                _isReadyToStart = true;
                _animator.SetBool("IsReadyToStart", true);
            }
        }

        public override void OnJoinedLobby()
        {
            _animator.SetBool("IsConnected", true);
        }

        private void LoadLobby()
        {
                ScenesStateMachine.ChangeScene(ScenesStates.Lobby);
        }
    }
}
