using System.Collections;
using System.Collections.Generic;

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
    [SerializeField] TMP_Text _speedText = default, _currentScoreText = default, _bestScoreText = default, _highestSpeedText = default, _goldText = default, _upgradeCostText = default, _currentAccelerationText = default;

    const string HIGHEST_SPEED = "highest_speed";
    float _currentSpeed, _highestSpeed;

    void Awake() {
        _gameManager.GameStarted += OnGameStarted;
        _gameManager.GameEnded += OnGameEnded;
        _upgradeController.UpgradeSucceeded += OnUpgradeSucceeded;
        _upgradeController.UpgradeFailed += OnUpgradeFailed;
        _upgradeController.MaxUpgraded += OnMaxUpgraded;
        
    }
    void Start() {
        _highestSpeed = PlayerPrefs.GetFloat(HIGHEST_SPEED, 0);
        _inGameMenu.SetActive(false);
        _mainMenu.SetActive(true);
        UpdateMainMenuTexts();
        //UpdateUpgradeMenuTexts();
    }
    
    void Update() {
        _currentSpeed = _playerRb.velocity.magnitude;
        _speedText.text = "Speed:" + _currentSpeed.ToString("0.00") + " kmph";
        if (_currentSpeed > _highestSpeed)
            _highestSpeed = _currentSpeed;
    }

    void OnDestroy() {
        _gameManager.GameStarted -= OnGameStarted;
        _gameManager.GameEnded -= OnGameEnded;
        _upgradeController.UpgradeSucceeded -= OnUpgradeSucceeded;
        _upgradeController.UpgradeFailed -= OnUpgradeFailed;
        _upgradeController.MaxUpgraded -= OnMaxUpgraded;
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
        UpdateMainMenuTexts();
    }

    void OnGameStarted() {
        _inGameMenu.SetActive(true);
        _mainMenu.SetActive(false);
    }

    void UpdateMainMenuTexts() {
        _highestSpeedText.text = "Highest Speed:" + _highestSpeed.ToString("0.00") + " kmph";
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

}
