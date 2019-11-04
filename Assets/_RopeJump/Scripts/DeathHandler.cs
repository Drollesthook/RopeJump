using System;
using UnityEngine;

public class DeathHandler : MonoBehaviour {
    public event Action PlayerDead; 
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Ground")) {
            PlayerDead?.Invoke();
        }
    }
}
