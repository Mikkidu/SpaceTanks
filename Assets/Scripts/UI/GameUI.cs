using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AlexDev.SpaceTanks
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinText;

        private void Start()
        {
            GameManager.OnCoinsChangeEvent += UpdateCoins;
        }

        private void UpdateCoins(int coinsAmount)
        {
            if (_coinText != null)
                _coinText.SetText(coinsAmount.ToString());
            else
                Debug.LogError("<Color=Red><a>Missing</a></Color> coin text", this);
        }
    }
}
