using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AlexDev.SpaceTanks
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Slider _healthBar;
        [SerializeField] private TextMeshProUGUI _playerNameText;
        [SerializeField] private Vector3 _screenOffset = new Vector3(0, 30, 0);
        [SerializeField] private float _characterControllerHeight;

        private PlayerHealth _target;
        private int _maxHealth;
        private Transform _targetTransform;
        private Renderer _targetRenderer;
        private CanvasGroup _canvasGroup;
        Vector3 _targetPosition;

        private void Awake()
        {
            this.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetTarget(PlayerHealth target)
        {
            if (target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayerMoveManager target for PlayerUI.SetTarget", this);
                return;
            }

            _target = target;
            _maxHealth = target.GetMaxHealth;
            target.OnHealthChangedEvent += UpdateHealthBar;
            if (_playerNameText != null)
                _playerNameText.text = target.photonView.Owner.NickName;
            _targetTransform = _target.GetComponent<Transform>();
            _targetRenderer = _target.GetComponent<Renderer>();
        }

        private void LateUpdate()
        {
            if (_targetRenderer != null)
            {
                _canvasGroup.alpha = _targetRenderer.isVisible ? 1f : 0f;
            }

            if (_targetTransform != null)
            {
                _targetPosition = _targetTransform.position;
                _targetPosition.y += _characterControllerHeight;
                transform.position = Camera.main.WorldToScreenPoint(_targetPosition) + _screenOffset;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void UpdateHealthBar(int currentHealth)
        {
            _healthBar.value = currentHealth / (float)_maxHealth;
        }
    }
}
