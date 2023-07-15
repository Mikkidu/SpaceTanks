using UnityEngine;

using Photon.Pun;

namespace AlexDev.SpaceTanks
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviourPun
    {
        [SerializeField] private float _shootForce;

        private int _damage;
        private int _ownerID;

        void Start()
        {
            DestroyBullet(2);
            if (!photonView.IsMine && PhotonNetwork.IsConnected)
                return;
            GetComponent<Rigidbody2D>().AddForce(transform.up * _shootForce, ForceMode2D.Impulse);
        }

        public void Initialization(int damage, int ownerID)
        {
            _damage = damage;
            _ownerID = ownerID;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected)
                return;
            if (collision.TryGetComponent<UnitHealth>(out UnitHealth target))
            {
                if (target.TakeDamage(_damage, _ownerID))
                {
                    photonView.RPC("DestroyBullet", RpcTarget.All, 0f);
                }
            }
        }

        [PunRPC]
        private void DestroyBullet(float lifeTime)
        {
            Destroy(gameObject, lifeTime);
        }
    }
}
