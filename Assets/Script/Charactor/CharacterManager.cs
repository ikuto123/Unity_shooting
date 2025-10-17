using UnityEngine;
using System;

public class CharacterManager : MonoBehaviour, IChargeable , IDamageable , IRecover
{
    public WeaponManager WeaponManager { get; private set; }
    public bool isPlayer = false;
    public event Action<int, int> OnHpChanged;
    public event Action<int, int> OnChargeChanged;
    
    public event Action OnPlayerDied;
    public event Action OnPlayerRespawned;
    public int MaxHp { get; private set; }
    private int _currentHp;
    public float MoveSpeed { get; private set; }
    public float RespawnDelay { get; private set; }
    
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
            Debug.Log($"<color=cyan>Energy Updated:</color> {gameObject.name} の現在エネルギーは {_currentCharge} / {MaxCharge} です。");
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
        
        MoveSpeed = initStats.moveSpeed;
        MaxHp = initStats.maxHp;
        MaxCharge = initStats.maxCharge;
        CurrentHp = MaxHp;
        CurrentCharge = MaxCharge;
        RespawnDelay = initStats.respawnDelay;
        
    }
    
    public void ResetStatus()
    {
        CurrentHp = MaxHp;
        CurrentCharge = MaxCharge;
        
        if (isPlayer)
        {
            OnPlayerRespawned?.Invoke();
        }
    }

    public void TakeDamage(int damage)
    {
        CurrentHp -= damage;
    }
    
    public void RecoverHp(int amount)
    {
        if (CurrentHp >= MaxHp)
        {
            CurrentHp = MaxHp;
            return;
        }
        CurrentHp += amount;
        Debug.Log($"{amount} HP回復！ 現在のHP: {CurrentHp}");
    }
    
    public void RecoverCharge(int amount)
    {
        if (CurrentCharge >= MaxCharge)
        {
            CurrentCharge = MaxCharge;
            return;
        }
        CurrentCharge += amount;
        Debug.Log($"{amount} チャージ回復！ 現在のチャージ: {CurrentCharge}");
    }

    private void OnEnable()
    {
        // もしプレイヤーなら、カメラをプレイヤーカメラに戻す
        if (isPlayer && GameManager.Instance != null)
        {
            GameManager.Instance.SwitchToPlayerCamera();
        }
    }
    
    private void Die()
    {
        if (this == null) return;
        if (isPlayer && GameManager.Instance != null)
        {
            OnPlayerDied?.Invoke();
            GameManager.Instance.SwitchToStageCamera();
        }
        transform.gameObject.SetActive(false);
        GameManager.Instance.SpawnManager.RequestRespawn(this);
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