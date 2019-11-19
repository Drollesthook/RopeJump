using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour {
    [SerializeField] GameObject _player = default, _destroyedPlayer = default;
    [SerializeField] Color _greenColor = default, _redColor = default, _yellowColor = default, _blueColor = default, _purpleColor = default, _blackColor = default;
    
    List<Material> _listOfDeadMaterials = new List<Material>();
    Material _playerMaterial, _trailMaterial;
    
    void Start() {
        GetPlayerMaterial();
        GetPlayerPartsMaterials();
    }

    void GetPlayerMaterial() {
        _playerMaterial = _player.GetComponentInChildren<MeshRenderer>().material;
        _trailMaterial = _player.GetComponent<TrailRenderer>().material;
    }

    void GetPlayerPartsMaterials() {
        foreach (MeshRenderer meshRenderer in _destroyedPlayer.GetComponentsInChildren<MeshRenderer>()) {
            _listOfDeadMaterials.Add(meshRenderer.material);
            print("ayoo");
        }
    }
    
    public void SetGreenColor() {
        SetColor(_greenColor);
    }

    public void SetRedColor() {
        SetColor(_redColor);
    }

    public void SetYellowColor() {
        SetColor(_yellowColor);
    }
    
    public void SetBlueColor() {
        SetColor(_blueColor);
    }
    
    public void SetPurpleColor() {
        SetColor(_purpleColor);
    }
    
    public void SetBlackColor() {
        SetColor(_blackColor);
    }

    void SetColor(Color color) {
        for (int i = 0; i < _listOfDeadMaterials.Count; i++) {
            _listOfDeadMaterials[i].color = color;
        }
        _playerMaterial.color = color;
        _trailMaterial.color = color;
    }
}
