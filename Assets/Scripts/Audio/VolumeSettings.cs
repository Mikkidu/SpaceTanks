using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace AlexDev.SpaceTanks
{
    public class VolumeSettings : MonoBehaviour
    {
        public static VolumeSettings instance;
        [SerializeField] private AudioMixer _mixer;

        public static bool isMusicOn = true;
        public static bool isSfxOn = true;

        private GameSettingsData _gameSettings;
        private AudioManager _audioManager;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            InitializeVolume();
        }

        public void InitializeVolume()
        {
            _gameSettings = DataManager.instance.gameSettings;
            _audioManager = AudioManager.instance;
            SetMusicVolume(_gameSettings.musicVolume);
            SetSFXVolume(_gameSettings.sfxVolume);
            _audioManager.musicSource.mute = !_gameSettings.isMusicOn;
            _audioManager.sfxSource.mute = !_gameSettings.isSfxOn;
        }

        public void SetMusicVolume(float volume)
        {
            _mixer.SetFloat(Constants.SETTINGS_VOLUME_MUSIC,  Mathf.Log10(volume) * 20);
            _gameSettings.musicVolume = volume;
        }

        public void SetSFXVolume(float volume)
        {
            _mixer.SetFloat(Constants.SETTINGS_VOLUME_SFX, Mathf.Log10(volume) * 20);
            _gameSettings.sfxVolume = volume;
        }
    
        public void SwitchOnMusic(bool isOn)
        {
            _audioManager.musicSource.mute = !isOn;
            _gameSettings.isMusicOn = isOn;
            _audioManager.PlaySound("tap");
            
        }
    
        public void SwitchOnSfx(bool isOn)
        {
            _audioManager.sfxSource.mute = !isOn;
            _gameSettings.isSfxOn = isOn;
            _audioManager.PlaySound("tap");
        }

    }
}
