using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlexDev.SpaceTanks
{
    public class SettingsUI : MonoBehaviour
    {

        private GameSettingsData _gameSettings;
        private VolumeSettings _volumeSettings;

        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Toggle _sfxToggle;


        private void Awake()
        {
            _volumeSettings = VolumeSettings.instance;
        }

        private void Start()
        {
            _musicSlider.onValueChanged.AddListener(_volumeSettings.SetMusicVolume);
            _sfxSlider.onValueChanged.AddListener(_volumeSettings.SetSFXVolume);
            _musicToggle.onValueChanged.AddListener(_volumeSettings.SwitchOnMusic);
            _sfxToggle.onValueChanged.AddListener(_volumeSettings.SwitchOnSfx);
            _gameSettings = DataManager.instance.gameSettings;
            LoadVolumeUI();
        }

        public void SaveSettings()
        {
            DataManager.instance.SaveGameSettings();
            AudioManager.instance.PlaySound("button");
        }
        /*
        public void SaveMusicVolume()
        {
            _gameSettings.musicVolume = _musicSlider.value;
        }

        public void SaveSfxVolume()
        {
            _gameSettings.sfxVolume = _sfxSlider.value;
        }
        */
        private void LoadVolumeUI()
        {
            _musicSlider.value = _gameSettings.musicVolume;
            _sfxSlider.value = _gameSettings.sfxVolume;
            _musicSlider.interactable = _gameSettings.isMusicOn;
            _musicToggle.isOn = _gameSettings.isMusicOn;
            _sfxSlider.interactable = _gameSettings.isSfxOn;
            _sfxToggle.isOn = _gameSettings.isSfxOn;
        }
    }
}
