using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI _timerText;

    private void Awake()
    {
        
        _timerText = GetComponent<TextMeshProUGUI>();
    }
    
    private void Start()
    {
        GameManager.Instance.OnTimeChanged += UpdateTimerText;
     }
    
    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnTimeChanged -= UpdateTimerText;
        }
    }
    
    private void UpdateTimerText(float remainingTime)
    {
        
        remainingTime = Mathf.Max(0, remainingTime);
        
        
        _timerText.text = remainingTime.ToString("F1");
    }
}
