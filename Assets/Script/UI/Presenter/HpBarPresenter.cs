using UnityEngine;
using UnityEngine.UI;

public class HpBarPresenter
{
    private readonly CharacterManager _model;
    private readonly Image _view;

    // 司令塔からモデルとビューを注入してもらう
    public HpBarPresenter(CharacterManager model, Image view)
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
        Debug.Log("PlayerHP" + ((float)currentHp / maxHp));
        _view.fillAmount = (float)currentHp / maxHp;
    }
}
