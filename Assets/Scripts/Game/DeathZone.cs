using UnityEngine;

using Photon.Pun;

namespace AlexDev.SpaceTanks
{
    public class DeathZone : MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _hitInterval = 0.5f;

        private float _nextHitTime;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            if (collision.TryGetComponent<UnitHealth>(out UnitHealth target))
            {
                if (_nextHitTime < Time.realtimeSinceStartup)
                {
                    target.TakeDamage(_damage);
                }
            }
        }
    }
}
