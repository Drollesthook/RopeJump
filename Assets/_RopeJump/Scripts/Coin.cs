using UnityEngine;

public class Coin : MonoBehaviour
{
    void Update() {
        transform.Rotate(Vector3.up * 100 * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
            gameObject.SetActive(false);
    }
}
