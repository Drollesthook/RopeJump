using UnityEngine;

using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour {
    [SerializeField] GameObject _player = default;
    [SerializeField] GameObject _joint = default;
    [SerializeField] float _forcePower = default;
    [SerializeField] float _ceilingPositionY = default;
    [SerializeField] float _Xoffset = default;
    [SerializeField] LayerMask _ceilingMask = default;

    Rigidbody _playerRb;
    Vector3 _raycastDirection;
    Vector3 _hookPosition;
    LineRenderer _lineRenderer;
    DeathHandler _deathHandler;
    bool _isTouchReseted;

    Vector3 _playerStartPos, _hookStartPos;
    void Start() {
        GetStartPositions();
        _deathHandler = _player.GetComponent<DeathHandler>();
        _lineRenderer = _player.GetComponent<LineRenderer>();
        _playerRb = _player.GetComponent<Rigidbody>();
        _lineRenderer.SetPosition(0, _player.transform.position);
        _lineRenderer.SetPosition(1, _player.transform.position);
        _lineRenderer.enabled = false;
        CountRaycastDirection();
        _deathHandler.PlayerDead += OnPlayerDead;
    }

    void OnDestroy() {
        _deathHandler.PlayerDead -= OnPlayerDead;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            GetHookPosition();
            ReleaseHook();
            _isTouchReseted = true;
        }
        if (Input.GetMouseButton(0) && _isTouchReseted) {
            DrawLineRenderer();
            AddForceToPlayer();
        }
        if (Input.GetMouseButtonUp(0)) {
            HideHook();
        }
    }

    void GetStartPositions() {
        _playerStartPos = _player.transform.position;
        _hookStartPos = _joint.transform.position;
    }

    void Reset() {
        HideHook();
        _isTouchReseted = false;
        _playerRb.velocity = Vector3.zero;
        _hookPosition = _hookStartPos;
        _player.transform.position = _playerStartPos;
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

    void OnPlayerDead() {
        Reset();
    }

    void GetHookPosition() {
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
