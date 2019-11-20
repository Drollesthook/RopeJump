using System.Collections;

using TMPro;
using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    [SerializeField] GameManager _gameManager = default;
    [SerializeField] GameObject _mainMenu = default, _inGameMenu = default;
    [SerializeField] Rigidbody _playerRb = default;
    [SerializeField] SoftCurrencyController _softCurrencyController = default;
    [SerializeField] UpgradeController _upgradeController = default;
    [SerializeField] Color _defaultGoldTextColor = default, _warningGoldTextColor = default;
    [SerializeField] Button _upgradeButton = default;
    [SerializeField] float _checkForSpeedAndDistanceDelay = 0.1f;
    [SerializeField] TMP_Text _speedText = default,
                              _DistanceText = default,
                              _highestDistanceText = default,
                              _highestSpeedText = default,
                              _goldText = default,
                              _upgradeCostText = default,
                              _currentAccelerationText = default;

    const string HIGHEST_SPEED = "highest_speed";
    const string HIGHEST_DISTANCE = "highest_distance";
    float _currentSpeed, _highestSpeed, _currentDistance, _highestDistance;
    DeathHandler _deathHandler;

    bool _isGameStarted;

    void Awake() {
        _deathHandler = _playerRb.GetComponent<DeathHandler>();
        _deathHandler.PlayerDead += OnPlayerDied;
        _gameManager.GameStarted += OnGameStarted;
        _gameManager.GameEnded += OnGameEnded;
        _upgradeController.UpgradeSucceeded += OnUpgradeSucceeded;
        _upgradeController.UpgradeFailed += OnUpgradeFailed;
        _upgradeController.MaxUpgraded += OnMaxUpgraded;
        
    }
    void Start() {
        _highestSpeed = PlayerPrefs.GetFloat(HIGHEST_SPEED, 0);
        _highestDistance = PlayerPrefs.GetFloat(HIGHEST_DISTANCE, 0);
        _inGameMenu.SetActive(false);
        _mainMenu.SetActive(true);
        UpdateMainMenuTexts();
    }
    

    void OnDestroy() {
        _deathHandler.PlayerDead -= OnPlayerDied;
        _gameManager.GameStarted -= OnGameStarted;
        _gameManager.GameEnded -= OnGameEnded;
        _upgradeController.UpgradeSucceeded -= OnUpgradeSucceeded;
        _upgradeController.UpgradeFailed -= OnUpgradeFailed;
        _upgradeController.MaxUpgraded -= OnMaxUpgraded;
    }

    void UpdateSpeedAndDistanceInfo() {
        _currentSpeed = _playerRb.velocity.magnitude;
        _currentDistance = _playerRb.transform.position.x;
        _speedText.text = "Speed:" + _currentSpeed.ToString("0.00") + " kmph";
        _DistanceText.text = " " + _currentDistance.ToString("0.00") + "m";
        if (_currentSpeed > _highestSpeed)
            _highestSpeed = _currentSpeed;
        
        if (_currentDistance > _highestDistance)
            _highestDistance = _currentDistance;
    }

    void OnUpgradeSucceeded() {
        UpdateMainMenuTexts();
        UpdateUpgradeMenuTexts();
    }

    void OnMaxUpgraded() {
        _upgradeButton.interactable = false;
        _upgradeCostText.text = "Upgraded!";
    }

    void OnUpgradeFailed() {
        BlinkGoldTextWithRedColor();
    }
    
    void OnGameEnded() {
        _inGameMenu.SetActive(false);
        _mainMenu.SetActive(true);
        if(_highestSpeed > PlayerPrefs.GetFloat(HIGHEST_SPEED, 0))
            PlayerPrefs.SetFloat(HIGHEST_SPEED, _highestSpeed);
        if(_highestDistance > PlayerPrefs.GetFloat(HIGHEST_DISTANCE, 0))
            PlayerPrefs.SetFloat(HIGHEST_DISTANCE, _highestDistance);
        UpdateMainMenuTexts();
    }

    void OnPlayerDied() {
        _isGameStarted = false;
        StopCoroutine(CheckSpeedAndDistanceWithDelay());
    }

    void OnGameStarted() {
        _inGameMenu.SetActive(true);
        _mainMenu.SetActive(false);
        _isGameStarted = true;
        StartCoroutine(CheckSpeedAndDistanceWithDelay());
    }

    void UpdateMainMenuTexts() {
        _highestSpeedText.text = "Highest Speed:" + _highestSpeed.ToString("0.00") + " kmph";
        _highestDistanceText.text = "Best Distance:" + _highestDistance.ToString("0.00") + "m";
        _goldText.text = _softCurrencyController.Amount().ToString();
    }

    void UpdateUpgradeMenuTexts() {
        _upgradeCostText.text = "Upgrade:" + _upgradeController.CurrentUpgradeCost;
        _currentAccelerationText.text = _upgradeController.CurrentAcceleration + " HP";
    }

    void BlinkGoldTextWithRedColor() {
        _goldText.color = _warningGoldTextColor;
        _goldText.DOColor(_defaultGoldTextColor, 1);
    }

    IEnumerator CheckSpeedAndDistanceWithDelay() {
        while (_isGameStarted) {
            UpdateSpeedAndDistanceInfo();
            yield return new WaitForSeconds(_checkForSpeedAndDistanceDelay);
        }
    }
}
