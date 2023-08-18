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
            PlayersStatsManager.instance.OnPlayerStateChangedEvent += UpdatePlayerList;
            PlayersStatsManager.instance.OnCoinsChangeEvent += UpdateCoins;
        }

        private void UpdatePlayerList()
        {
            _leaderboardText.text = PlayersStatsManager.instance.GetFullPlayersStates();
        }

        private void UpdateCoins(int coinsAmount)
        {
            _coinText.text = coinsAmount.ToString();
        }

        public void ShowWinPanel()
        {
            _leaderboardText.transform.parent.parent.gameObject.SetActive(false);
            string hederText = $"{PlayersStatsManager.instance.GetFirsLivePlayerName(true)} won this battle!";
            string leadersTableText = PlayersStatsManager.instance.GetFullPlayersStates();
            _winPanel.SetActive(true);
            _winPanel.GetComponent<LeadersPanelUI>().Initialization(hederText, leadersTableText);
        }

        public void StartGame()
        {
            _waitingPanel.SetActive(false);
        }
        
        public void OnDisable()
        {
            PlayersStatsManager.instance.OnPlayerStateChangedEvent -= UpdatePlayerList;
            PlayersStatsManager.instance.OnCoinsChangeEvent -= UpdateCoins;
        }
    }
}
