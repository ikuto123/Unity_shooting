using GameScene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MunuButtonPresenter
{
    private readonly GameManager _gameManager;
    private readonly Button _RestartButton;
    private readonly Button _TitleButton;
    private readonly Button _MapButton;
    private readonly Button _HowToPlayButton;
    private readonly Button _QuitMapAndHowToPlayButton;
    
    private GameObject _MapPanel;
    private GameObject _HowToPlayPanel;
    
    private SoundData _clickSE;
    public MunuButtonPresenter(GameManager gameManager, Button RestartButton,
        Button MapButton, Button TitleButton , Button HowToPlayButton, Button QuitMapAndHowToPlayButton,
        GameObject MapPanel , GameObject HowToPlayPanel , SoundData clickSE)
    {
        _gameManager = gameManager;
        _RestartButton = RestartButton;
        _TitleButton = TitleButton;
        _MapButton = MapButton;
        _MapPanel = MapPanel;
        _HowToPlayButton = HowToPlayButton;
        _QuitMapAndHowToPlayButton = QuitMapAndHowToPlayButton;
        _HowToPlayPanel = HowToPlayPanel;
        _clickSE = clickSE;
    }
        
    public void Enable()
    {
        _RestartButton.onClick.AddListener(OnRestart);
        _TitleButton.onClick.AddListener(OnBackToTitle);
        _MapButton.onClick.AddListener(OnShowMap);
        _QuitMapAndHowToPlayButton.onClick.AddListener(OnQuitMapAndHowToPlay);
        _HowToPlayButton.onClick.AddListener(OnShowHowToPlay);
    }
    
    public void Disable()
    {
        _RestartButton.onClick.RemoveListener(OnRestart);
        _TitleButton.onClick.RemoveListener(OnBackToTitle);
        _MapButton.onClick.RemoveListener(OnShowMap);
        _QuitMapAndHowToPlayButton.onClick.RemoveListener(OnQuitMapAndHowToPlay);
        _HowToPlayButton.onClick.RemoveListener(OnShowHowToPlay);
    }
    
    private void OnRestart()
    {
        SoundManager.Instance.PlaySE_2D(_clickSE);
        _gameManager.PauseMenu();
    }

    private void OnBackToTitle()
    {
        Time.timeScale = 1f;
        SoundManager.Instance.PlaySE_2D(_clickSE);
        SceneManager.LoadScene("Title");
    }

    private void OnShowMap()
    {
        _MapPanel.gameObject.SetActive(true); 
        SoundManager.Instance.PlaySE_2D(_clickSE);
        _QuitMapAndHowToPlayButton.gameObject.SetActive(true);
    }

    private void OnShowHowToPlay()
    {
        _HowToPlayPanel.gameObject.SetActive(true); 
        SoundManager.Instance.PlaySE_2D(_clickSE);
        _QuitMapAndHowToPlayButton.gameObject.SetActive(true);
    }
    private void OnQuitMapAndHowToPlay()
    {
        _MapPanel.gameObject.SetActive(false);
        _HowToPlayPanel.gameObject.SetActive(false);
        _QuitMapAndHowToPlayButton.gameObject.SetActive(false);
        SoundManager.Instance.PlaySE_2D(_clickSE);
    }
    
}
