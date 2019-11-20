using System.Collections.Generic;

using Lean.Pool;

using UnityEngine;

public class LevelBlock : MonoBehaviour, IPoolable
{
    List<Vector3> WallsPos = new List<Vector3>();
    List<WallOfBlocks> listOfWalls = new List<WallOfBlocks>();
    List<Coin> _listOfCoins = new List<Coin>();
    void Start()
    {
        WallsPos.Clear();
        GetAllWallsPositions();
        GetAllCoins();
    }

    public void OnSpawn() {
        
    }

    public void OnDespawn() {
        ReturnWallsOnPositions();
        ReturnCoins();
    }
    
    void GetAllCoins() {
        foreach (Coin coin in gameObject.GetComponentsInChildren<Coin>()) {
            _listOfCoins.Add(coin);
        }
    }

    void GetAllWallsPositions() {
        foreach (WallOfBlocks wall in gameObject.GetComponentsInChildren<WallOfBlocks>()) {
            listOfWalls.Add(wall);
            WallsPos.Add(wall.transform.localPosition);
        }
    }
    
    void ReturnCoins() {
        for (int i = 0; i < _listOfCoins.Count; i++) {
            _listOfCoins[i].gameObject.SetActive(true);
        }
    }

    void ReturnWallsOnPositions() {
        for (int i = 0; i < listOfWalls.Count; i++) {
            listOfWalls[i].transform.localPosition = WallsPos[i];
            listOfWalls[i].ReturnObstaclesOnPositions();
        }
    }
    
}
