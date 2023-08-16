using System.Collections;
using System;
using UnityEngine;

[Serializable]
public class GameSettingsData
{
    public float musicVolume;
    public bool isMusicOn;
    public float sfxVolume;
    public bool isSfxOn;
    public GameSettingsData()
    {
        musicVolume = 0.5f;
        isMusicOn = true;
        sfxVolume = 0.5f;
        isSfxOn = true;
    }
}
