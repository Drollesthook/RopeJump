using System.Collections;
using System.Collections.Generic;

using Lean.Pool;

using UnityEngine;

public class GroundSpawnController : MonoBehaviour {
    [SerializeField] GameObject _levelBlock = default;
    [SerializeField] Transform _player = default;
    [SerializeField] float _blockLength = default;

    DeathHandler _deathHandler;
    int _blocksSpawned;
    float _levelLength, _offset;
    Queue<GameObject> _blocksQueue = new Queue<GameObject>();
    void Start() {
        _deathHandler = _player.GetComponent<DeathHandler>();
        _deathHandler.PlayerDead += OnPlayerDead;
        _offset = _blockLength;
        StartCoroutine(CheckForDespawnTimer());
    }

    void OnDestroy() {
        _deathHandler.PlayerDead -= OnPlayerDead;
    }

    void Update(){
        CheckForNeedToSpawn();
    }

    void CheckForNeedToSpawn() {
        if(_levelLength - _offset > _player.position.x)
            return;
        SpawnNextBlock();
    }

    void SpawnNextBlock() {
        //_blocksQueue.Enqueue(Instantiate(_levelBlock, CountBlockSpawnPos(), Quaternion.identity));
        _blocksQueue.Enqueue(LeanPool.Spawn(_levelBlock, CountBlockSpawnPos(), Quaternion.identity));
        _blocksSpawned++;
        _levelLength = _blocksSpawned * _blockLength;
    }

    void CheckForNeedToDespawn() {
        if(_blocksQueue == null)
            return;
        if (_blocksQueue.Peek().transform.position.x + _offset < _player.position.x) {
            DespawnBlock();
        }
    }

    void OnPlayerDead() {
        Reset();
    }

    void Reset() {
        for(int i = 0; i <= _blocksQueue.Count; i++) {
            LeanPool.Despawn(_blocksQueue.Dequeue());
            //Destroy(_blocksQueue.Dequeue());
        }
        _blocksSpawned = 0;
        _levelLength = 0;
    }

    void DespawnBlock() {
        LeanPool.Despawn(_blocksQueue.Dequeue());
        //Destroy(_blocksQueue.Dequeue());
    }

    Vector3 CountBlockSpawnPos() {
        return new Vector3(_blocksSpawned * _blockLength, 0, 0);
    }

    IEnumerator CheckForDespawnTimer() {
        while (true) {
        yield return new WaitForSeconds(0.5f);
            CheckForNeedToDespawn();
        }
    }
}
