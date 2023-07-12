using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexDev.SpaceTanks
{
    public class DataManager : MonoBehaviour
    {
        [Tooltip("Player name panel")]
        [SerializeField]
        private InputPanelUI _playerNamePanel;

        private void Start()
        {
            _playerNamePanel.OnNameAcceptedEvent += ChangeName;
        }

        public void ChangeName(string name)
        {
            PlayerPrefs.SetString(Constants.PLAYER_NAME_PREF_KEY, name);
        }
    }
}
