using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class UIController : MonoBehaviour {
    [SerializeField] GameManager _gameManager = default;
    [SerializeField] GameObject _mainMenu = default, _inGameMenu = default;
    [SerializeField] Rigidbody _playerRb = default;
    [SerializeField] TMP_Text _speedText = default, _currentScoreText = default, _bestScoreText = default;
    void Start()
    {
        _inGameMenu.SetActive(false);
        _mainMenu.SetActive(true);
        _gameManager.GameStarted += OnGameStarted;
        _gameManager.GameEnded += OnGameEnded;
    }

    void OnDestroy() {
        _gameManager.GameStarted -= OnGameStarted;
        _gameManager.GameEnded -= OnGameEnded;
    }
    void OnGameEnded() {
        _inGameMenu.SetActive(false);
        _mainMenu.SetActive(true);
    }

    void OnGameStarted() {
        _inGameMenu.SetActive(true);
        _mainMenu.SetActive(false);
    }

    void Update() {
        _speedText.text = "Speed:" + _playerRb.velocity.magnitude.ToString("0.00");
    }
}
