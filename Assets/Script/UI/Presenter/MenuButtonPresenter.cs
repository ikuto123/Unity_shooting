using GameScene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonPresenter
{
    private readonly GameManager _gameManager;
    private readonly Button _restartButton;
    private readonly Button _titleButton;
    private readonly Button _mapButton;
    private readonly Button _howToPlayButton;
    private readonly Button _quitMapAndHowToPlayButton;
    
    private GameObject _mapPanel;
    private GameObject _howToPlayPanel;
    
    private SoundData _clickSE;
    public MenuButtonPresenter(GameManager gameManager, Button RestartButton,
        Button MapButton, Button TitleButton , Button HowToPlayButton, Button QuitMapAndHowToPlayButton,
        GameObject MapPanel , GameObject HowToPlayPanel , SoundData clickSE)
    {
        _gameManager = gameManager;
        _restartButton = RestartButton;
        _titleButton = TitleButton;
        _mapButton = MapButton;
        _mapPanel = MapPanel;
        _howToPlayButton = HowToPlayButton;
        _quitMapAndHowToPlayButton = QuitMapAndHowToPlayButton;
        _howToPlayPanel = HowToPlayPanel;
        _clickSE = clickSE;
    }
        
    public void Enable()
    {
        _restartButton.onClick.AddListener(OnRestart);
        _titleButton.onClick.AddListener(OnBackToTitle);
        _mapButton.onClick.AddListener(OnShowMap);
        _quitMapAndHowToPlayButton.onClick.AddListener(OnQuitMapAndHowToPlay);
        _howToPlayButton.onClick.AddListener(OnShowHowToPlay);
    }
    
    public void Disable()
    {
        _restartButton.onClick.RemoveListener(OnRestart);
        _titleButton.onClick.RemoveListener(OnBackToTitle);
        _mapButton.onClick.RemoveListener(OnShowMap);
        _quitMapAndHowToPlayButton.onClick.RemoveListener(OnQuitMapAndHowToPlay);
        _howToPlayButton.onClick.RemoveListener(OnShowHowToPlay);
    }
    
    //再開ボタン
    private void OnRestart()
    {
        SoundManager.Instance.PlaySE_2D(_clickSE);
        _gameManager.PauseMenu();
    }

    //タイトルに戻る
    private void OnBackToTitle()
    {
        Time.timeScale = 1f;
        SoundManager.Instance.PlaySE_2D(_clickSE);
        SceneManager.LoadScene("Title");
    }

    //マップの表示
    private void OnShowMap()
    {
        _mapPanel.gameObject.SetActive(true); 
        SoundManager.Instance.PlaySE_2D(_clickSE);
        _quitMapAndHowToPlayButton.gameObject.SetActive(true);
    }

    //遊び方の表示
    private void OnShowHowToPlay()
    {
        _howToPlayPanel.gameObject.SetActive(true); 
        SoundManager.Instance.PlaySE_2D(_clickSE);
        _quitMapAndHowToPlayButton.gameObject.SetActive(true);
    }
    //マップと遊び方を閉じる
    private void OnQuitMapAndHowToPlay()
    {
        _mapPanel.gameObject.SetActive(false);
        _howToPlayPanel.gameObject.SetActive(false);
        _quitMapAndHowToPlayButton.gameObject.SetActive(false);
        SoundManager.Instance.PlaySE_2D(_clickSE);
    }
    
}
