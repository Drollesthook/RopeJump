using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] Transform _player = default;

    Vector3 _offset;
    void Start() {
        _offset = transform.position - _player.position;
    }

    void Update() {
        transform.position = _player.position + _offset;
    }
}
