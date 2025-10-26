using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Unity.Cinemachine;

namespace GameScene
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        [SerializeField] private CinemachineCamera _playerCamera;
        [SerializeField] private CinemachineCamera _playerFPSCamera;

        [SerializeField] private CinemachineCamera _stageCamera1;
        [SerializeField] private CinemachineCamera _stageCamera2;
        [SerializeField] private AudioSource _worldSource;

        public CharacterSpawnManager SpawnManager { get; private set; }
        public OccupationManager OccupationManager { get; private set; }
        public RecoveryAreaManager RecoveryManager { get; private set; }
        public float AreaTimeToWin { get; private set; }

        [Header("ゲーム設定")] private float _remainingTime;
        private bool _isGameActive = false;
        public bool IsGameActive => _isGameActive;
        public event Action<float> OnTimeChanged;
        public event Action OnGameStart;

        private float _startTimer = 5f;
       
        private bool _isPaused = false;
        public bool IsPaused => _isPaused;
        public event Action<bool> OnPauseStateChanged;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _isGameActive = true;

            LoadGameSettings();

            SpawnManager = GetComponent<CharacterSpawnManager>();
            OccupationManager = GetComponent<OccupationManager>();

            RecoveryManager = new RecoveryAreaManager();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private async void Start()
        {
            await StartStandTimer();//ゲーム開始前のカウントダウンタイマー
            _worldSource.Play();//ゲーム開始時にBGM再生
            SpawnManager.SpawnCharacters();//キャラクターのスポーン
        }
        
        
        private async UniTask StartStandTimer()
        {
            OnTimeChanged?.Invoke(_startTimer);
            _isGameActive = false; 
            
            while (_startTimer > 0)
            {
                _startTimer -= Time.deltaTime;
                OnTimeChanged?.Invoke(_startTimer); 
                
                await UniTask.Yield(); 
            }

            OnTimeChanged?.Invoke(0f); 
            OnGameStart?.Invoke();
            _isGameActive = true;
        }

        //Jsonファイルからゲームに必要な設定を読み込む
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
            if (!_isGameActive || _isPaused) return;

            //ゲーム中ののカウントダウン
            if (_remainingTime > 0)
            {
                _remainingTime -= Time.deltaTime;
                
                OnTimeChanged?.Invoke(_remainingTime);
            }
            else
            {
                _remainingTime = 0;
                _isGameActive = false;

                //現在の占領進捗から勝者を確定
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

        //ステージのカメラに切り替える
        public void SwitchToStageCamera()
        {
            if (_playerCamera != null) _playerCamera.gameObject.SetActive(false);
            int randomIndex = UnityEngine.Random.Range(0, 2);

            if (randomIndex == 0)
            {
                if (_stageCamera1 != null) _stageCamera1.gameObject.SetActive(true);
                if (_stageCamera2 != null) _stageCamera2.gameObject.SetActive(false);
            }
            else
            {
                if (_stageCamera1 != null) _stageCamera1.gameObject.SetActive(false);
                if (_stageCamera2 != null) _stageCamera2.gameObject.SetActive(true);
            }
        }

        //Playerのカメラに切り替える
        public void SwitchToPlayerCamera()
        {
            if (_playerCamera != null) _playerCamera.gameObject.SetActive(true);
            if (_playerFPSCamera != null) _playerFPSCamera.gameObject.SetActive(false);

            if (_stageCamera1 != null) _stageCamera1.gameObject.SetActive(false);
            if (_stageCamera2 != null) _stageCamera2.gameObject.SetActive(false);
        }


        //メニューが押されたときの処理
        public void PauseMenu()
        {
            if (!_isGameActive) return;

            _isPaused = !_isPaused;
            
            if (_isPaused)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {

                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            OnPauseStateChanged?.Invoke(_isPaused);
        }
    }
}