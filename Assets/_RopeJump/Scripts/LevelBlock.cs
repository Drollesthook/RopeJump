using System.Collections.Generic;

using Lean.Pool;

using UnityEngine;

public class LevelBlock : MonoBehaviour, IPoolable
{
    List<Vector3> WallsPos = new List<Vector3>();
    List<WallOfBlocks> listOfWalls = new List<WallOfBlocks>();
    void Start()
    {
        WallsPos.Clear();
        GetAllWallsPositions();
    }

    public void OnSpawn() {
        
    }

    public void OnDespawn() {
        ReturnWallsOnPositions();
    }

    void GetAllWallsPositions() {
        foreach (WallOfBlocks wall in gameObject.GetComponentsInChildren<WallOfBlocks>()) {
            listOfWalls.Add(wall);
            WallsPos.Add(wall.transform.localPosition);
        }
    }

    void ReturnWallsOnPositions() {
        for (int i = 0; i < listOfWalls.Count; i++) {
            listOfWalls[i].transform.localPosition = WallsPos[i];
            listOfWalls[i].ReturnObstaclesOnPositions();
        }
    }
    
}
