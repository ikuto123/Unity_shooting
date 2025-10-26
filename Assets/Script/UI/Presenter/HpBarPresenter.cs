using UnityEngine;
using UnityEngine.UI;

public class HpBarPresenter
{
    private readonly CharacterManager _model;
    private readonly Image _view;
    
    public HpBarPresenter(CharacterManager model, Image view)
    {
        _model = model;
        _view = view;
    }
    
    public void Enable()
    {
        _model.OnHpChanged += UpdateHpBar;
        UpdateHpBar(_model.CurrentHp, _model.MaxHp);
    }
    
    public void Disable()
    {
        _model.OnHpChanged -= UpdateHpBar;
    }

    private void UpdateHpBar(int currentHp, int maxHp)
    {
        _view.fillAmount = (float)currentHp / maxHp;
    }
}
