using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AlexDev.SpaceTanks
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private TextMeshProUGUI _leaderboardText;
        [SerializeField] private GameObject _winPanel;

        private void Start()
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

        public void ShowWinPanel()
        {
            Debug.Log("PlayerDie");
            _winPanel.SetActive(true);
            _leaderboardText.transform.parent.gameObject.SetActive(false);
            string hederText = $"{PlayersStatsManager.Instance.GetFirsLivePlayerName(true)} won this battle!";
            string leadersTableText = PlayersStatsManager.Instance.GetFullPlayersStates();
            _winPanel.GetComponent<LeadersPanelUI>().Initialization(hederText, leadersTableText);
        }
    }
}
