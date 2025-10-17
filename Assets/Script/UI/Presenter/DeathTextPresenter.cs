using UnityEngine;
using TMPro;
using System.Collections; // Coroutineを使うために必要

public class DeathTextPresenter
{
    private readonly CharacterManager _model;
    private readonly GameObject _viewGameObject; // テキストを含むGameObject全体
    private readonly TextMeshProUGUI _textView;
    private readonly MonoBehaviour _coroutineRunner; // コルーチン実行のため

    // アニメーション設定
    private readonly Color _darkRed;
    private readonly Color _lightRed;
    private readonly float _pulsateSpeed;
    private Coroutine _pulsateCoroutine;

    public DeathTextPresenter(CharacterManager model, GameObject viewGameObject, MonoBehaviour coroutineRunner, Color dark, Color light, float speed)
    {
        _model = model;
        _viewGameObject = viewGameObject;
        _textView = viewGameObject.GetComponentInChildren<TextMeshProUGUI>(); // 子から取得
        _coroutineRunner = coroutineRunner;
        _darkRed = dark;
        _lightRed = light;
        _pulsateSpeed = speed;
    }

    public void Enable()
    {
        // プレイヤーのイベントを購読
        if (_model != null && _model.isPlayer)
        {
            _model.OnPlayerDied += Show;
            _model.OnPlayerRespawned += Hide;
        }
        Hide(); // 初期状態は非表示
    }

    public void Disable()
    {
        // 購読解除
        if (_model != null && _model.isPlayer)
        {
            _model.OnPlayerDied -= Show;
            _model.OnPlayerRespawned -= Hide;
        }
        Hide(); // 無効化時も非表示にする
    }

    private void Show()
    {
        if (_viewGameObject != null)
        {
            _viewGameObject.SetActive(true);
            if (_pulsateCoroutine != null) _coroutineRunner.StopCoroutine(_pulsateCoroutine);
            _pulsateCoroutine = _coroutineRunner.StartCoroutine(PulsateText());
        }
    }

    private void Hide()
    {
        if (_viewGameObject != null)
        {
            if (_pulsateCoroutine != null) _coroutineRunner.StopCoroutine(_pulsateCoroutine);
            _pulsateCoroutine = null;
            _viewGameObject.SetActive(false);
        }
    }

    private IEnumerator PulsateText()
    {
        if (_textView == null) yield break;
        float timer = 0f;
        while (true)
        {
            timer += Time.deltaTime * _pulsateSpeed;
            float lerpFactor = Mathf.PingPong(timer, 1f);
            _textView.color = Color.Lerp(_darkRed, _lightRed, lerpFactor);
            yield return null;
        }
    }
}