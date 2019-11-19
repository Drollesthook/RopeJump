using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public event Action GameStarted, GameEnded, GamePaused, GameContinued;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartGame() {
        GameStarted?.Invoke();
    }

    public void EndGame() {
        GameEnded?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
