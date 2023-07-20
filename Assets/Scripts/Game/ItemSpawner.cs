using UnityEngine;

using Photon.Pun;

namespace AlexDev.SpaceTanks
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _coinPrefab;
        [SerializeField] private Transform _bottomLeftCorner;
        [SerializeField] private Transform _upperRightCorner;
        [SerializeField] private float _spawnInterval;

        private float _spawnTimer = 0;

        void Awake()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Update()
        {
            if (Time.realtimeSinceStartup > _spawnTimer)
            {
                SpawnCoin();
            }
        }

        private void SpawnCoin()
        {
            if (_coinPrefab == null)
                return;
            float randomX = Random.Range(_bottomLeftCorner.position.x, _upperRightCorner.position.x);
            float randomY = Random.Range(_bottomLeftCorner.position.y, _upperRightCorner.position.y);
            Vector2 spawnPosition = new Vector2(randomX, randomY);
            PhotonNetwork.Instantiate(_coinPrefab.name, spawnPosition, Quaternion.identity);
            _spawnTimer = Time.realtimeSinceStartup + _spawnInterval;
        }

    }
}
