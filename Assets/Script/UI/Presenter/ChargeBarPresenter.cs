using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBarPresenter
{
    private readonly CharacterManager _model;
    private readonly Image _view;

    public ChargeBarPresenter(CharacterManager model, Image view)
    {
        _model = model;
        _view = view;
    }

    public void Enable()
    {
        _model.OnChargeChanged += UpdateChargeSlider;
        UpdateChargeSlider(_model.CurrentCharge, _model.MaxCharge);
    }

    public void Disable()
    {
        _model.OnChargeChanged -= UpdateChargeSlider;
    }

    private void UpdateChargeSlider(int currentCharge, int maxCharge)
    {
        Debug.Log("PlayerHP" + ((float)currentCharge / maxCharge));
        _view.fillAmount = (float)currentCharge / maxCharge;
    }
}
