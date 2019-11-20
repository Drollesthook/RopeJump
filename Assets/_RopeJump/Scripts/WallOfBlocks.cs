using System.Collections.Generic;
using UnityEngine;

public class WallOfBlocks : MonoBehaviour
{
    List<Vector3> _obstaclesPos = new List<Vector3>();
    List<Obstacle> _listOfObstacles = new List<Obstacle>();
    List<Rigidbody> _listOfObstaclesRb = new List<Rigidbody>();
    void Start()
    {
        GetAllObstacles();
    }

    void GetAllObstacles() {
        foreach (Obstacle obstacle in gameObject.GetComponentsInChildren<Obstacle>()) {
            _listOfObstaclesRb.Add(obstacle.GetComponent<Rigidbody>());
            _listOfObstacles.Add(obstacle);
            _obstaclesPos.Add(obstacle.transform.localPosition);
        }
    }


    public void ReturnObstaclesOnPositions() {
        for (int i = 0; i < _listOfObstacles.Count; i++) {
            _listOfObstaclesRb[i].angularVelocity = Vector3.zero;
            _listOfObstaclesRb[i].velocity = Vector3.zero;
            _listOfObstacles[i].transform.localPosition = _obstaclesPos[i];
            _listOfObstacles[i].transform.eulerAngles = Vector3.zero;
        }
    }
}
