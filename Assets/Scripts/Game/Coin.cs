using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

namespace AlexDev.SpaceTanks
{
    public class Coin : MonoBehaviourPun
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player"))
                return;
            PhotonView targetPhoton = collision.GetComponent<PhotonView>();
            if (targetPhoton.IsMine)
            {
                PlayersStatsManager.Instance.AddCoins(targetPhoton.ViewID, 1);
                photonView.RPC("DestroyCoin", RpcTarget.All);
            }
        }

        [PunRPC]
        private void DestroyCoin()
        {
            Destroy(gameObject);
        }
    }
}
