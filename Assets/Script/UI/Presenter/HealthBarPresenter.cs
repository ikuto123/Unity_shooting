using UnityEngine;
using UnityEngine.UI;

public class HealthBarPresenter
{
    private readonly CharacterManager _model;
    private readonly Slider _view;

    // 司令塔からモデルとビューを注入してもらう
    public HealthBarPresenter(CharacterManager model, Slider view)
    {
        _model = model;
        _view = view;
    }

    // 監視を開始する
    public void Enable()
    {
        _model.OnHpChanged += UpdateHpBar;
        // 初期表示
        UpdateHpBar(_model.CurrentHp, _model.MaxHp);
    }

    // 監視を終了する
    public void Disable()
    {
        _model.OnHpChanged -= UpdateHpBar;
    }

    private void UpdateHpBar(int currentHp, int maxHp)
    {
        _view.value = (float)currentHp / maxHp;
    }
}
