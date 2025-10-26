using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class HowToPlayUI : MonoBehaviour
{
    [SerializeField] private GameObject _pagePearent;
    [SerializeField] private SoundData _clickSE;
    
    private float slideDuration = 0.4f;
    private float TargetPagePos;

    [SerializeField] private Button _leftArrowButton;
    [SerializeField] private Button _rightArrowButton;

    private float _MaxPagePos = 0;
    private float _MinPagePos = -3760;
    
    private bool _isSliding;
    private void Awake()
    {
        _leftArrowButton.onClick.AddListener(OnLeftArrowClick);
        _rightArrowButton.onClick.AddListener(OnRightArrowClick);
    }

    private void OnEnable()
    {
        TargetPagePos = 0;
        _pagePearent.transform.localPosition = new Vector2(TargetPagePos, _pagePearent.transform.localPosition.y);
        _isSliding = true;
        UpdateArrowButton();
    }
    
    private async void StartSlide()
    {
        float CurrentPagePosX = _pagePearent.transform.localPosition.x;
        await SlideTimer(CurrentPagePosX);
        
    }

    //ページがスライドする処理
    private async UniTask SlideTimer(float CurrentPagePosX)
    {
        Vector2 startPosition = _pagePearent.transform.localPosition;
        Vector2 targetPosition = new Vector2(TargetPagePos, startPosition.y);
        float timer = 0f;
        _isSliding = false;
        UpdateArrowButton();
        
        while (timer < slideDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, timer / slideDuration); 
            _pagePearent.transform.localPosition = Vector2.Lerp(startPosition, targetPosition, t);
            Debug.Log("ループ中");
            await UniTask.Yield();
        }
        _pagePearent.transform.localPosition = targetPosition;
        
        _isSliding = true;
        UpdateArrowButton();
    }
    
    public void OnRightArrowClick()
    {
        TargetPagePos += -1880;
        SoundManager.Instance.PlaySE_2D(_clickSE);
        StartSlide();
    }

    public void OnLeftArrowClick()
    {
        TargetPagePos -= -1880;
        SoundManager.Instance.PlaySE_2D(_clickSE);
        StartSlide();
    }
    
    //矢印ボタンが押せるかどうか
    private void UpdateArrowButton()
    {
        if (!_isSliding)
        {
            _leftArrowButton.interactable = false;
            _rightArrowButton.interactable = false;
        }
        else
        {
            _leftArrowButton.interactable = (_MaxPagePos > TargetPagePos);
            _rightArrowButton.interactable = (_MinPagePos < TargetPagePos);
        }

    }

}