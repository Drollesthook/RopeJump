using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {
    [SerializeField] GameManager _gameManager = default;
    [SerializeField] GameObject _solidPlayer = default, _destroyedPlayer = default;
    [SerializeField] float _deathExplosionForce = 10f;
    DeathHandler _deathHandler;

    List<Transform> _listOfPlayerParts = new List<Transform>();
    List<Vector3> _listOfPartsPos = new List<Vector3>();
    List<Rigidbody> _listOfPartsRb = new List<Rigidbody>();
    Vector3 _deathVelocity;
    void Start() {
        GetPlayerParts();
        _deathHandler = GetComponent<DeathHandler>();
        _deathHandler.PlayerDead += OnPlayerDied;
        _gameManager.GameEnded += OnGameEnded;
    }

    void OnDestroy() {
        _deathHandler.PlayerDead -= OnPlayerDied;
        _gameManager.GameEnded -= OnGameEnded;
    }

    void OnPlayerDied() {
        _deathVelocity = _solidPlayer.GetComponentInParent<Rigidbody>().velocity;
        _solidPlayer.SetActive(false);
        _destroyedPlayer.SetActive(true);
        AddForceToParts();
    }

    void OnGameEnded() {
        ReturnPartsOnPos();
        _destroyedPlayer.SetActive(false);
        _solidPlayer.SetActive(true);
    }

    void GetPlayerParts() {
        foreach (Transform child in _destroyedPlayer.GetComponentsInChildren<Transform>()) {
            _listOfPlayerParts.Add(child);
            _listOfPartsPos.Add(child.localPosition);
            _listOfPartsRb.Add(child.GetComponent<Rigidbody>());
        }
    }

    void ReturnPartsOnPos() {
        for (int i = 0; i < _listOfPlayerParts.Count; i++) {
            if(_listOfPartsRb[i] != null)
                _listOfPartsRb[i].velocity = Vector3.zero;
            _listOfPlayerParts[i].localPosition = _listOfPartsPos[i];
        }
    }

    void AddForceToParts() {
        for (int i = 0; i < _listOfPlayerParts.Count; i++) {
            if(_listOfPartsRb[i] != null)
                _listOfPartsRb[i].AddExplosionForce(_deathExplosionForce, _destroyedPlayer.transform.position, 2);
                //_listOfPartsRb[i].velocity = _deathVelocity;
        }
    }
}
