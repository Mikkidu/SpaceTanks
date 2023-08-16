using UnityEngine;
using TMPro;

namespace AlexDev.SpaceTanks
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private TextMeshProUGUI _leaderboardText;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _waitingPanel;

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
            _winPanel.SetActive(true);
            _leaderboardText.transform.parent.parent.gameObject.SetActive(false);
            string hederText = $"{PlayersStatsManager.Instance.GetFirsLivePlayerName(true)} won this battle!";
            string leadersTableText = PlayersStatsManager.Instance.GetFullPlayersStates();
            _winPanel.GetComponent<LeadersPanelUI>().Initialization(hederText, leadersTableText);
        }

        public void StartGame()
        {
            _waitingPanel.SetActive(false);
        }
    }
}
