using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class UIController : MonoBehaviour {
    [SerializeField] GameManager _gameManager = default;
    [SerializeField] GameObject _mainMenu = default, _inGameMenu = default;
    [SerializeField] Rigidbody _playerRb = default;
    [SerializeField] CurrencyController _currencyController = default;
    [SerializeField] TMP_Text _speedText = default, _currentScoreText = default, _bestScoreText = default, _highestSpeedText = default, _goldText = default;

    const string HIGHEST_SPEED = "highest_speed";
    float _currentSpeed, _highestSpeed;
    void Start() {
        _highestSpeed = PlayerPrefs.GetFloat(HIGHEST_SPEED, 0);
        _inGameMenu.SetActive(false);
        _mainMenu.SetActive(true);
        _gameManager.GameStarted += OnGameStarted;
        _gameManager.GameEnded += OnGameEnded;
        UpdateMainMenuTexts();
    }

    void OnDestroy() {
        _gameManager.GameStarted -= OnGameStarted;
        _gameManager.GameEnded -= OnGameEnded;
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
        _goldText.text = _currencyController.Amount().ToString();
    }

    void Update() {
        _currentSpeed = _playerRb.velocity.magnitude;
        _speedText.text = "Speed:" + _currentSpeed.ToString("0.00") + " kmph";
        if (_currentSpeed > _highestSpeed)
            _highestSpeed = _currentSpeed;
    }
}
