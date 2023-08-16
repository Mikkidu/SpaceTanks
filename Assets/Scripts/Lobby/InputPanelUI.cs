using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AlexDev.SpaceTanks
{
    public class InputPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputText;
        [SerializeField] private string _defaultNameConstant;
        [SerializeField] private Button _acceptButton;
        [SerializeField] private bool _needConnection;

        public delegate void OnNameAccepted(string name);
        public OnNameAccepted OnNameAcceptedEvent;

        public bool _isReadyToInput;

        private void Start()
        {
            if (_needConnection)
            {
                ConnectionManager.Instance.OnMasterConnectionChangeEvent += OnReadyStateChange;
                OnReadyStateChange(ConnectionManager.Instance.IsConnectedToMaster);
            }
            if (_inputText != null)
            {
                if (PlayerPrefs.HasKey(_defaultNameConstant))
                    _inputText.text = PlayerPrefs.GetString(_defaultNameConstant);
            }
        }

        public void OnReadyStateChange(bool isReady)
        {
            _isReadyToInput = isReady;
            CheckInputField(_inputText.text);
        }

        public void OnButtonClick()
        {
            if (OnNameAcceptedEvent != null)
                OnNameAcceptedEvent.Invoke(_inputText.text);
            AudioManager.instance.PlaySound("button");
        }

        public void CheckInputField(string name)
        {
            if (_isReadyToInput && string.IsNullOrEmpty(name) == _acceptButton.interactable)
            {
                _acceptButton.interactable = !_acceptButton.interactable;
            }
            AudioManager.instance.PlaySound("tap");
        }

    }
}
