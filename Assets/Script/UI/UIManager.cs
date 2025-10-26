using System.Collections;
using GameScene;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("監視対象のモデル")]
    [SerializeField] private CharacterManager _characterManager;
    [SerializeField] private AreaControl _areaControl;
    [SerializeField] private CharactorTeam _playerTeam;
    [SerializeField] private GameManager _gameManager;

    [Header("操作対象のビュー（UI要素）")]
    [SerializeField] private Image _hpSlider;
    [SerializeField] private Image _energySlider;
    [SerializeField] private GameObject _gunSelectUI;
    [SerializeField] private Image _teamASlider;
    [SerializeField] private Image _teamBSlider;
    [SerializeField] private GameObject PlayerDieTexts;
    [Header("リザルトビュー")]
    [SerializeField] private Image _PlayerWin;
    [SerializeField] private Image _PlayerLose;
    [SerializeField] private TextMeshProUGUI _DrawText;
    [Header("死亡テキスト設定")]
    [SerializeField] private GameObject _playerDieTextsGO; 
    [SerializeField] private Color _darkRed = new Color(0.5f, 0, 0);
    [SerializeField] private Color _lightRed = Color.red;
    [SerializeField] private float _pulsateSpeed = 2f;

    [Header("ゲームスタート待機テキスト")] 
    [SerializeField] private GameObject _gameStartUI;
    
    [Header("ポーズメニュー")]
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mapButton;
    [SerializeField] private Button _titleButton;
    [SerializeField] private Button _howToPlayButton;
    [SerializeField] private Button _quitMapButton;
    [SerializeField] private GameObject _mapPanel;
    [SerializeField] private GameObject _howToPlayPanel;
    
    [Header("ターゲット")]
    [SerializeField] private GameObject _target_TPS;
    [SerializeField] private GameObject _target_FPS;
    private CameraModeChanger _cameraModeChanger;
   
    private HpBarPresenter _healthBarPresenter;
    private ChargeBarPresenter _chargeBarPresenter;
    private GunSelectPresenter _gunSelectPresenter;
    private AreaPresenter _areaPresenter;
    private ResultPresenter _resultPresenter;
    private DeathTextPresenter _deathTextPresenter;
    private MenuButtonPresenter _menuButtonPresenter;
    private TargetPointPresenter _targetPointPresenter;
    
    [SerializeField] private SoundData _clickSE;
    
    private void Awake()
    {
        //Presenterの初期化
        _healthBarPresenter = new HpBarPresenter(_characterManager, _hpSlider);
        _chargeBarPresenter = new ChargeBarPresenter(_characterManager, _energySlider);
        _gunSelectPresenter = new GunSelectPresenter(_characterManager, _gunSelectUI);

        _areaPresenter = new AreaPresenter(_areaControl, _teamASlider, _teamBSlider);
        _resultPresenter = new ResultPresenter(_areaControl, _PlayerWin, _PlayerLose, _playerTeam.Team, _DrawText);
        _deathTextPresenter = new DeathTextPresenter(_characterManager, _playerDieTextsGO, _darkRed,_lightRed,_pulsateSpeed);
        _menuButtonPresenter = new MenuButtonPresenter(_gameManager, _restartButton, _mapButton, _titleButton,_howToPlayButton, _quitMapButton, _mapPanel,_howToPlayPanel , _clickSE);
        _pauseMenuPanel.SetActive(false);
    }

    private void Start()
    {
        _healthBarPresenter.Enable();
        _chargeBarPresenter.Enable();
        _gunSelectPresenter.Enable();
        _areaPresenter.Enable();
        _resultPresenter.Enable();
        _deathTextPresenter.Enable();
        _menuButtonPresenter.Enable();
        _gameManager.OnPauseStateChanged += HandlePauseStateChanged;
        _areaControl.OnGameEnd += HandleGameEnd;
        _gameManager.OnGameStart += HandleGameStarted;
        _gameStartUI.SetActive(true);
        
        //これだけ初期化のタイミングずらさないとnullになる(コンストラクタでイベントを登録)
        var playerInput = FindObjectOfType<PlayerInputController>();
        _cameraModeChanger = playerInput?.GetCameraModeChanger();
        _targetPointPresenter = new TargetPointPresenter(_cameraModeChanger , _target_TPS , _target_FPS);
    }

    private void OnDisable()
    {
        _healthBarPresenter.Disable();
        _chargeBarPresenter.Disable();
        _gunSelectPresenter.Disable();
        _areaPresenter.Disable();
        _resultPresenter.Disable();
        _deathTextPresenter.Disable();
        _menuButtonPresenter.Disable();
        _areaControl.OnGameEnd -= HandleGameEnd;
    }
    
    private void HandleGameEnd(Team? _)
    {
        Time.timeScale = 0f; 
        StartCoroutine(ReturnToTitleAfterDelay(3f));
    }

    private IEnumerator ReturnToTitleAfterDelay(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }
    
    private void HandleGameStarted()
    {
        if (_gameStartUI != null)
        {
            _gameStartUI.SetActive(false);
        }
    }
    
    private void HandlePauseStateChanged(bool isPaused)
    {
        if (_pauseMenuPanel != null)
        {
            _pauseMenuPanel.SetActive(isPaused);
        }
    }
}
