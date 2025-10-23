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

    [Header("UIグループ")]
    [SerializeField]
    private GameObject _taitleGroups; 

    [SerializeField]
    private GameObject _mainMenuGroup;  

    [Header("最初に選択するボタン")]
    [SerializeField]
    private GameObject _firstSelectedButton; 
    
    void Start()
    {
        _currentState = TitleState.PressEnter;
        
        _taitleGroups.SetActive(true);  
        _mainMenuGroup.SetActive(false); 
    }
    
    void Update()
    {
        if (_currentState != TitleState.PressEnter) return;

        if (Input.GetKey(KeyCode.Return))
        {
            ShowMainMenu();
        }
    }
    
    private void ShowMainMenu()
    {
        _currentState = TitleState.MainMenu;
        
        _taitleGroups.SetActive(false);
        _mainMenuGroup.SetActive(true);
        
        EventSystem.current.SetSelectedGameObject(_firstSelectedButton);
    }
    
    public void OnStartButtonClick()
    {
        Debug.Log("ゲームスタート処理");
        GameManager.Instance.StartGame();

    }
    
    public void OnHowToPlayButtonClick()
    {
        Debug.Log("遊び方シーンへ遷移");
        GameManager.Instance.HowToPlay();
    }

    public void OnQuitHowToPlayButtonClick()
    {
        GameManager.Instance.QuitHowToPlay();
    }
    
    public void OnQuitButtonClick()
    {
        Debug.Log("ゲーム終了処理");
        GameManager.Instance.QuitGame();
    }
}
