using UnityEngine;

public class CurrencyController : MonoBehaviour {
    [SerializeField] GameManager _gameManager = default;

    const string GOLD_AMOUNT = "gold_amount";
    int _currentGold;
    
    void Start()
    {
        _currentGold = PlayerPrefs.GetInt(GOLD_AMOUNT, 0);
        _gameManager.GameEnded += SaveGoldAmount;
    }

    void OnDestroy() {
        _gameManager.GameEnded -= SaveGoldAmount;
    }

    public void AddCoin() {
        _currentGold++;
    }

    public int Amount() {
        return _currentGold;
    }

    public void Spend(int amount) {
        _currentGold -= amount;
        SaveGoldAmount();
    }

    void SaveGoldAmount() {
        PlayerPrefs.SetInt(GOLD_AMOUNT, _currentGold);
    }
}
