using System;
using UnityEngine;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private CinemachineCamera playerCamera;
    
    [SerializeField] private CinemachineCamera stageCamera1;
    [SerializeField] private CinemachineCamera stageCamera2;

    public CharacterSpawnManager SpawnManager { get; private set; }
    public OccupationManager OccupationManager { get; private set; }
    public RecoveryAreaManager RecoveryManager { get; private set; }
    public float AreaTimeToWin { get; private set; }

    [Header("ゲーム設定")]
    private float _remainingTime;
    private bool _isGameActive = false;
    public bool IsGameActive => _isGameActive;
    public event Action<float> OnTimeChanged;
    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        _isGameActive = true;

        LoadGameSettings();
        
        SpawnManager = GetComponent<CharacterSpawnManager>();
        OccupationManager = GetComponent<OccupationManager>();
        
        RecoveryManager = new RecoveryAreaManager();
    }

    private void Start()
    {
        SpawnManager.SpawnCharacters();
    }
    
    private void LoadGameSettings()
    {
        var jsonTextAsset = Resources.Load<TextAsset>("Data/GameMasterData");
        if (jsonTextAsset != null)
        {
            GameSettingsData settings = JsonUtility.FromJson<GameSettingsData>(jsonTextAsset.text);
            _remainingTime = settings.gameDuration;
            AreaTimeToWin = settings.areaTimeToWin;
            
        }
        else
        {
            Debug.LogWarning("GameSettingsData.jsonが見つかりません。インスペクターのデフォルト値を使用します。");
        }
    }
    
    private void Update()
    {
        if (!_isGameActive) return;

        if (_remainingTime > 0)
        {
            _remainingTime -= Time.deltaTime;
            // ★ 時間が更新されたら、イベントを発行して通知する
            OnTimeChanged?.Invoke(_remainingTime);
        }
        else
        {
            _remainingTime = 0;
            _isGameActive = false;

            // ★ 時間切れ：現在の占領進捗から勝者（または引き分け）を確定
            var area = FindObjectOfType<AreaControl>();
            if (area != null)
            {
                area.ForceEndByTimeUp();
            }
            else
            {
                Debug.LogWarning("AreaControlが見つからず、時間切れの勝敗判定を実行できませんでした。");
            }
        }
    }
    
    public void SwitchToStageCamera()
    {
        if (playerCamera != null) playerCamera.gameObject.SetActive(false);
        int randomIndex = UnityEngine.Random.Range(0, 2);

        if (randomIndex == 0)
        {
            if (stageCamera1 != null) stageCamera1.gameObject.SetActive(true);
            if (stageCamera2 != null) stageCamera2.gameObject.SetActive(false);
        }
        else
        {
            if (stageCamera1 != null) stageCamera1.gameObject.SetActive(false);
            if (stageCamera2 != null) stageCamera2.gameObject.SetActive(true);
        }
    }
    
    public void SwitchToPlayerCamera()
    {
        if (playerCamera != null) playerCamera.gameObject.SetActive(true);
        
        if (stageCamera1 != null) stageCamera1.gameObject.SetActive(false);
        if (stageCamera2 != null) stageCamera2.gameObject.SetActive(false);
    }
}
