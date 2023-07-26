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
            PhotonView playersPhoton = collision.GetComponent<PhotonView>();
            if (playersPhoton.IsMine)
            {
                PlayersStats.Instance.AddPlayerCoins(playersPhoton.ViewID, 1);
                //GameManager.AddCoins(1);
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
