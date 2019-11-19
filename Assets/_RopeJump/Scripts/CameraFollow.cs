using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour {
    [SerializeField] Transform _player = default;
    [SerializeField] DeathHandler _deathHandler = default;
    [SerializeField] GameManager _gameManager = default;

    Vector3 _offset;
    void Start() {
        _offset = transform.position - _player.position;
        _deathHandler.PlayerDead += OnPlayerDied;
        _gameManager.GameEnded += OnGameEnded;
    }

    void OnDestroy() {
        _deathHandler.PlayerDead -= OnPlayerDied;
        _gameManager.GameEnded -= OnGameEnded;
    }

    void LateUpdate() {
        transform.position = _player.position + _offset;
    }

    void OnPlayerDied() {
        transform.DOMove(new Vector3(transform.position.x + 5, transform.position.y, transform.position.z),
                         _deathHandler.DeathDelay * 0.9f);
        gameObject.GetComponent<CameraFollow>().enabled = false;
    }

    void OnGameEnded() {
        gameObject.GetComponent<CameraFollow>().enabled = true;
    }
}
