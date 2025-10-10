using UnityEngine;
using System;

public class CharacterManager : MonoBehaviour, IChargeable , IDamageable
{
    public WeaponManager WeaponManager { get; private set; }
    
    public event Action<int, int> OnHpChanged;
    public event Action<int, int> OnChargeChanged;
    
    public int MaxHp { get; private set; }
    private int _currentHp;
    public int CurrentHp
    {
        get => _currentHp;
        private set
        {
            _currentHp = Mathf.Clamp(value, 0, MaxHp);
            OnHpChanged?.Invoke(_currentHp, MaxHp);
            if (_currentHp <= 0) Die();
        }
    }

    public int MaxCharge { get; private set; }
    private int _currentCharge;
    public int CurrentCharge
    {
        get => _currentCharge;
        private set
        {
            _currentCharge = Mathf.Clamp(value, 0, MaxCharge);
            OnChargeChanged?.Invoke(_currentCharge, MaxCharge);
        }
    }

    private void Awake()
    {
        WeaponManager = new WeaponManager();
        SetInitData();
    }

    //キャラクターの初期値を代入
    private void SetInitData()
    {
        TextAsset statsJson = Resources.Load<TextAsset>("Data/CaractorData");

        CharacterStatsData initStats = JsonUtility.FromJson<CharacterStatsData>(statsJson.text);
        
        MaxHp = initStats.maxHp;
        MaxCharge = initStats.maxCharge;
        CurrentHp = MaxHp;
        CurrentCharge = MaxCharge;
    }

    public void TakeDamage(int damage)
    {
        CurrentHp -= damage;
    }
    
    public void RecoverHp(int amount)
    {
        CurrentHp += amount;
        Debug.Log($"{amount} HP回復！ 現在のHP: {CurrentHp}");
    }
    
    public void RecoverCharge(int amount)
    {
        CurrentCharge += amount;
        Debug.Log($"{amount} チャージ回復！ 現在のチャージ: {CurrentCharge}");
    }

    private void Die()
    {
        if (this == null) return;
        transform.gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} は倒れた。");
    }
    
    //エネルギーがあるかの確認
    public bool HasEnoughCharge(int cost)
    {
        return CurrentCharge >= cost; 
    }

    //銃を使った際に呼び出す
    public void UseCharge(int cost)
    {
        if (HasEnoughCharge(cost))
        {
            CurrentCharge -= cost;
        }
    }
}