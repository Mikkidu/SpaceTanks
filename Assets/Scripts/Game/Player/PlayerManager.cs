using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Photon.Pun;
using Photon.Realtime;

namespace AlexDev.SpaceTanks
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private SpriteRenderer _bodySprite;


        private void Awake()
        {
            if (photonView.IsMine)
            {
                PlayersStatsManager.SetPlayerViewID(photonView.ViewID);
            }
        }
        private void Start()
        {
            _bodySprite.color = PlayersStatsManager.instance.GetPlayerColor(photonView.Owner.UserId);
        }

        private void ChangeColor(Color newColor)
        {
            _bodySprite.color = newColor;
            Debug.Log(gameObject.name + newColor.ToString());
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            if (!changedProps.ContainsKey("Color"))
                return;
            if (targetPlayer.UserId == photonView.Owner.UserId)
            {
                ChangeColor(PlayerColors.GetRgbColor(changedProps["Color"].ToString()));
                Debug.Log(targetPlayer.NickName + " " + changedProps["Color"].ToString());
            }
        }
    }
}
