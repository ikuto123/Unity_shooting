using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("監視対象のモデル")]
    [SerializeField] private CharacterManager _characterManager;
    [SerializeField] private AreaControl _areaControl;
    [SerializeField] private CharactorTeam _playerTeam;

    [Header("操作対象のビュー（UI要素）")]
    [SerializeField] private Image _hpSlider;
    [SerializeField] private Image _energySlider;
    [SerializeField] private GameObject _gunSelectUI;
    [SerializeField] private Image _teamASlider;
    [SerializeField] private Image _teamBSlider;
    [Header("リザルトビュー")]
    [SerializeField] private Image _PlayerWin;
    [SerializeField] private Image _PlayerLose;
    [SerializeField] private TextMeshProUGUI _DrawText;
    
    // 専門家チームのメンバー
    private HpBarPresenter _healthBarPresenter;
    private ChargeBarPresenter _chargeBarPresenter;
    private GunSelectPresenter _gunSelectPresenter;
    private AreaPresenter _areaPresenter;
    private ResultPresenter _resultPresenter;
    
    private void Awake()
    {
        // 専門家たちを生成し、必要な部品（モデルとビュー）を渡す
        _healthBarPresenter = new HpBarPresenter(_characterManager, _hpSlider);
        _chargeBarPresenter = new ChargeBarPresenter(_characterManager, _energySlider);
        _gunSelectPresenter = new GunSelectPresenter(_characterManager, _gunSelectUI);

        _areaPresenter = new AreaPresenter(_areaControl, _teamASlider, _teamBSlider);
        _resultPresenter = new ResultPresenter(_areaControl, _PlayerWin, _PlayerLose, _playerTeam.Team, _DrawText);
    }

    private void Start()
    {
        _healthBarPresenter.Enable();
        _chargeBarPresenter.Enable();
        _gunSelectPresenter.Enable();
        _areaPresenter.Enable();
        _resultPresenter.Enable();
        _areaControl.OnGameEnd += HandleGameEnd;
    }

    private void OnDisable()
    {
        // 各専門家に監視を終了させる
        _healthBarPresenter.Disable();
        _chargeBarPresenter.Disable();
        _gunSelectPresenter.Disable();
        _areaPresenter.Disable();
        _resultPresenter.Disable();
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

        // 次のシーンへ行く前に時刻を戻す
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }
}
