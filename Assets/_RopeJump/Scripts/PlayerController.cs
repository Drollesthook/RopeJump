using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour {
    [SerializeField] GameObject _player = default;
    [SerializeField] GameObject _joint = default;
    [SerializeField] float _forcePower = default;
    [SerializeField] float _ceilingPositionY = default;
    [SerializeField] float _Xoffset = default;
    [SerializeField] LayerMask _ceilingMask = default;
    [SerializeField] GameManager _gameManager = default;

    Rigidbody _playerRb;
    Vector3 _raycastDirection;
    Vector3 _hookPosition;
    Vector3 _playerStartPos, _hookStartPos;
    LineRenderer _lineRenderer;
    TrailRenderer _trailRenderer;
    DeathHandler _deathHandler;
    bool _isTouchReseted, _isGameStarted, _isPlayerDead;

    const string ACCELERATION_POWER = "acceleration_power";

    void Start() {
        _deathHandler = _player.GetComponent<DeathHandler>();
        _lineRenderer = _player.GetComponent<LineRenderer>();
        _trailRenderer = _player.GetComponent<TrailRenderer>();
        _playerRb = _player.GetComponent<Rigidbody>();
        _deathHandler.PlayerDead += OnPlayerDied;
        _gameManager.GameStarted += OnGameStarted;
        _gameManager.GameEnded += OnGameEnded;
        CountRaycastDirection();
        _hookPosition = _raycastDirection;
        GetStartPositions();
        _lineRenderer.enabled = true;
        ReleaseHook();
    }

    void OnDestroy() {
        _deathHandler.PlayerDead -= OnPlayerDied;
        _gameManager.GameStarted -= OnGameStarted;
        _gameManager.GameEnded -= OnGameEnded;
    }

    void Update() {
        if (_isPlayerDead) return;
        if (_isGameStarted) {
            if (Input.GetMouseButtonDown(0)) {
                CountHookPosition();
                ReleaseHook();
                _isTouchReseted = true;
            }

            if (Input.GetMouseButton(0) && _isTouchReseted) {
                AddForceToPlayer();
            }

            if (Input.GetMouseButtonUp(0)) {
                HideHook();
            }
        }
        DrawLineRenderer();
    }

    void OnGameStarted() {
        _isGameStarted = true;
        _trailRenderer.enabled = true;
        _forcePower = PlayerPrefs.GetFloat(ACCELERATION_POWER, 40);
    }

    void OnGameEnded() {
        _isPlayerDead = false;
        _isGameStarted = false;
        Reset();
    }

    void GetStartPositions() {
        _playerStartPos = _player.transform.position;
        _hookStartPos = _hookPosition;
    }

    void Reset() {
        HideHook();
        _isTouchReseted = false;
        _playerRb.velocity = Vector3.zero;
        _hookPosition = _hookStartPos;
        _player.transform.position = _playerStartPos;
        ReleaseHook();
    }

    void ReleaseHook() {
        _joint.transform.position = _hookPosition;
        _joint.SetActive(true);
        _lineRenderer.enabled = true;
    }

    void HideHook() {
        _joint.SetActive(false);
        _lineRenderer.enabled = false;
    }

    void AddForceToPlayer() {
        _playerRb.AddForce(CountForceDirection()*_forcePower);
    }

    Vector3 CountForceDirection() {
        Vector3 hookDirection = _hookPosition - _player.transform.position;
        Vector3 dir = new Vector3(hookDirection.y, -hookDirection.x, hookDirection.z);
        return dir.normalized;
    }

    void CountRaycastDirection() {
        _raycastDirection = new Vector3(_player.transform.position.x + _Xoffset, _ceilingPositionY, 0);
    }

    void OnPlayerDied() {
        _isPlayerDead = true;
        _trailRenderer.enabled = false;
    }

    void CountHookPosition() {
        RaycastHit hit;
        if (Physics.Raycast(_player.transform.position, _raycastDirection, out hit, Mathf.Infinity, _ceilingMask)) {
            _hookPosition = hit.point;
        }
    }

    void DrawLineRenderer() {
        _lineRenderer.SetPosition(0, _player.transform.position);
        _lineRenderer.SetPosition(1, _hookPosition);
    }
}
