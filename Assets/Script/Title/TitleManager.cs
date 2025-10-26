using Taitle;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleManager : MonoBehaviour
{
    private enum TitleState
    {
        PressEnter, 
        MainMenu    
    }
    
    private TitleState _currentState;
    private TitleInput _titleInput;

    [Header("UIグループ")]
    [SerializeField]
    private GameObject _titleGroups; 

    [SerializeField]
    private GameObject _mainMenuGroup;  

    [Header("最初に選択するボタン")]
    [SerializeField]
    private GameObject _firstSelectedButton; 
    
    [Header("SE")]
    [SerializeField] private SoundData _ClickSE;
    void Start()
    {
        _currentState = TitleState.PressEnter;
        _titleInput = new TitleInput();
        _titleInput.OnEnterPressed += ShowMainMenu;
        
        _titleGroups.SetActive(true);  
        _mainMenuGroup.SetActive(false); 
    }
    
    void Update()
    {
        if (_currentState != TitleState.PressEnter) return;

        _titleInput.ReadInput();
    }
    
    private void ShowMainMenu()
    {
        _currentState = TitleState.MainMenu;
        
        _titleGroups.SetActive(false);
        _mainMenuGroup.SetActive(true);
        
        EventSystem.current.SetSelectedGameObject(_firstSelectedButton);
    }
    
    //ゲームスタート処理
    public void OnStartButtonClick()
    {
        SoundManager.Instance.PlaySE_2D(_ClickSE);
        GameManager.Instance.StartGame();

    }
    
    //遊び方シーンへ遷移
    public void OnHowToPlayButtonClick()
    {
        SoundManager.Instance.PlaySE_2D(_ClickSE);
        GameManager.Instance.HowToPlay();
    }

    public void OnQuitHowToPlayButtonClick()
    {
        GameManager.Instance.QuitHowToPlay();
    }
    
    //ゲーム終了処理
    public void OnQuitButtonClick()
    {
        SoundManager.Instance.PlaySE_2D(_ClickSE);
        GameManager.Instance.QuitGame();
    }
}
