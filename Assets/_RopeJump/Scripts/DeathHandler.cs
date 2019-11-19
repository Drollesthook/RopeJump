using System;
using System.Collections;

using UnityEngine;

public class DeathHandler : MonoBehaviour {
    [SerializeField] GameManager _gameManager = default;
    [SerializeField] float _deathDelay = 2f;

    public float DeathDelay => _deathDelay;
    public event Action PlayerDead; 
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("FatalGround")) {
            PlayerDead?.Invoke();
            StartCoroutine(DieWithDelay());
        }
    }

    IEnumerator DieWithDelay() {
        yield return new WaitForSeconds(_deathDelay);
        _gameManager.EndGame();
    }
}
