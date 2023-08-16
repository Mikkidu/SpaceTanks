using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace AlexDev.SpaceTanks
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager instance;

        public GameSettingsData gameSettings;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGameSattings();
        }

        public void SaveName(string name)
        {
            PlayerPrefs.SetString(Constants.PLAYER_NAME_PREF_KEY, name);
        }

        public void SaveGameSettings()
        {
            string json = JsonUtility.ToJson(gameSettings);
            Debug.Log("Save" + json);
            File.WriteAllText(Application.persistentDataPath + "/GameSettings.json", json);
        }


        public void LoadGameSattings()
        {
            string path = Application.persistentDataPath + "/GameSettings.json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                gameSettings = JsonUtility.FromJson<GameSettingsData>(json);
                return;
            }
            gameSettings = new GameSettingsData();
        }
    }
}
