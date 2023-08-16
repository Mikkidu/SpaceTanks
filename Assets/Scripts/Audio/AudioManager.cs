using UnityEngine.Audio;
using System;
using UnityEngine;


namespace AlexDev.SpaceTanks
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        public AudioSource musicSource;
        public AudioSource sfxSource;
        public Sound[] sounds;
        public Sound[] tracks;
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

        private void PlayMusic(Sound track)
        {
            musicSource.clip = track.clip;
            musicSource.Play();
        }

        public void PlayMusic(string trackName)
        {
            Sound track = Array.Find(tracks, tracks => tracks.Name == trackName);
            PlayMusic(track);
        }

        public void PlayMusicIfDifferent(string trackName)
        {
            Sound track = Array.Find(tracks, tracks => tracks.Name == trackName);
            if (track.clip == musicSource.clip)
                return;
            PlayMusic(track);
        }

        public void PlaySound(string soundName)
        {
            Sound sound = Array.Find(sounds, sounds => sounds.Name == soundName);
            if (sound != null)
                sfxSource.PlayOneShot(sound.clip, sound.volume);
        }
    }
}
