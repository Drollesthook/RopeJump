using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour {
    public event Action UpgradeSucceeded, UpgradeFailed, MaxUpgraded;

    [SerializeField] AccelerationStruct[] _accelerationStructs = default;
    [SerializeField] SoftCurrencyController _softCurrencyController = default;

    const string ACCELERATION_POWER = "acceleration_power";
    int _currentUpgradeCost;
    float _currentAcceleration;

    public int CurrentUpgradeCost => _currentUpgradeCost;

    public float CurrentAcceleration => _currentAcceleration;

    public Dictionary<int, int> _accelerationAndCost;

    [Serializable]
    struct AccelerationStruct {
        public int Cost;
        public float AccelerationPower;
    }

    void Start() {
        _currentAcceleration = PlayerPrefs.GetFloat(ACCELERATION_POWER, _accelerationStructs[0].AccelerationPower);
        UpdateCostInfo();
    }

    void UpdateCostInfo() {
        for (int i = 0; i < _accelerationStructs.Length; i++) {
            if (_currentAcceleration == _accelerationStructs[i].AccelerationPower) {
                if (i == _accelerationStructs.Length - 1) {
                    UpgradeSucceeded?.Invoke();
                    MaxUpgraded?.Invoke();
                    break;
                }
                _currentUpgradeCost = _accelerationStructs[i + 1].Cost;
                UpgradeSucceeded?.Invoke();
            }
        }
    }

    public void UpgradeButton() {
        for (int i = 0; i < _accelerationStructs.Length - 1; i++) {
            if (_currentAcceleration != _accelerationStructs[i].AccelerationPower)
            continue;
            {
                if (!_softCurrencyController.IsEnough(_currentUpgradeCost)) {
                    UpgradeFailed?.Invoke();
                    break;
                }

                _softCurrencyController.Spend(_currentUpgradeCost);
                _currentAcceleration = _accelerationStructs[i + 1].AccelerationPower;
                PlayerPrefs.SetFloat(ACCELERATION_POWER, _currentAcceleration);
                UpdateCostInfo();
                break;
            }
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
