using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AlexDev.SpaceTanks
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private TextMeshProUGUI _leaderboardText;

        private void Awake()
        {
            PlayersStatsManager.Instance.OnPlayerStateChangedEvent += UpdatePlayerList;
            PlayersStatsManager.Instance.OnCoinsChangeEvent += UpdateCoins;
        }

        private void UpdatePlayerList()
        {
            _leaderboardText.text = PlayersStatsManager.Instance.GetFullPlayersStates();
        }

        private void UpdateCoins(int coinsAmount)
        {
            _coinText.text = coinsAmount.ToString();
        }
    }
}
