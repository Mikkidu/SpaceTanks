using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Photon.Pun;
namespace AlexDev.SpaceTanks
{
    public class PlayerManager : MonoBehaviourPun
    {
        [SerializeField] private SpriteRenderer _bodySprite;


        private void Awake()
        {
            if (photonView.IsMine)
                PlayersStatsManager.SetPlayerViewID(photonView.ViewID);
        }
        private void Start()
        {
            _bodySprite.color = PlayersStatsManager.Instance.GetPlayerColor(photonView.Owner.UserId);
        }
    }
}
