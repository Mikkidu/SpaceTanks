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
            if (collision.GetComponent<PhotonView>().IsMine)
            {
                GameManager.AddCoins(1);
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
