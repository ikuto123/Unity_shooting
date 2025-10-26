using UnityEngine;
using TMPro;
using System.Threading;
using Cysharp.Threading.Tasks;

public class DeathTextPresenter
{
    private readonly CharacterManager _model;
    private readonly GameObject _viewGameObject; 
    private readonly TextMeshProUGUI _textView;
    
    private readonly Color _darkRed;
    private readonly Color _lightRed;
    private readonly float _pulsateSpeed;
  
    private CancellationTokenSource _cts;

    public DeathTextPresenter(CharacterManager model, GameObject viewGameObject, Color dark, Color light, float speed)
    {
        _model = model;
        _viewGameObject = viewGameObject;
        _textView = viewGameObject.GetComponentInChildren<TextMeshProUGUI>(); 
        _darkRed = dark;
        _lightRed = light;
        _pulsateSpeed = speed;
    }

    public void Enable()
    {
        if (_model != null && _model.IsPlayer)
        {
            _model.OnPlayerDied += Show;
            _model.OnPlayerRespawned += Hide;
        }
        Hide(); 
    }

    public void Disable()
    {
        if (_model != null && _model.IsPlayer)
        {
            _model.OnPlayerDied -= Show;
            _model.OnPlayerRespawned -= Hide;
        }
        Hide(); 
    }

    private void Show()
    {
        if (_viewGameObject != null)
        {
            _viewGameObject.SetActive(true);
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            
            PulsateText().Forget();
        }
    }

    private void Hide()
    {
        if (_viewGameObject == null) return;
        
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            _viewGameObject.SetActive(false);
        
    }

    //色を点滅させる
    private async UniTask PulsateText()
    {
        if (_textView == null) return;
        float timer = 0f;
        while (true)
        {
            timer += Time.deltaTime * _pulsateSpeed;
            float lerpFactor = Mathf.PingPong(timer, 1f);
            _textView.color = Color.Lerp(_darkRed, _lightRed, lerpFactor);
            await UniTask.Yield();
        }
    }
}