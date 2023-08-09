using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;

namespace AlexDev.SpaceTanks
{
    public class ScenesStateMachine : MonoBehaviour
    {
        private static ScenesStates _currentScene = ScenesStates.Loading;

        private void Awake()
        {
            System.Enum.TryParse<ScenesStates>(SceneManager.GetActiveScene().name, out _currentScene);
        }


        public static bool ChangeScene(ScenesStates sceneName)
        {
            switch (_currentScene)
            {
                case ScenesStates.Loading:
                    if (sceneName == ScenesStates.Lobby)
                    {
                        LoadScene(sceneName.ToString());
                        return false;
                    }
                    break;
                case ScenesStates.Lobby:
                    if (sceneName == ScenesStates.Game && Launcher.Instance != null)
                    {
                        PhotonNetwork.LoadLevel(sceneName.ToString());
                        return false;
                    }
                    break;
                case ScenesStates.Game:
                    if (sceneName == ScenesStates.Lobby)
                    {
                        LoadScene(sceneName.ToString());
                        return false;
                    }
                    break;
            }
            _currentScene = sceneName;
            return true;
        }

        private static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
