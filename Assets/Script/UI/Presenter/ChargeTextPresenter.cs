using TMPro;
using UnityEngine;

public class ChargeTextPresenter
{
    private readonly CharacterManager _model;
    private readonly TextMeshProUGUI _view;

    public ChargeTextPresenter(CharacterManager model, TextMeshProUGUI view)
    {
        _model = model;
        _view = view;
    }

    public void Enable()
    {
        _model.OnChargeChanged += UpdateChargeText;
        UpdateChargeText(_model.CurrentCharge, _model.MaxCharge);
    }

    public void Disable()
    {
        _model.OnChargeChanged -= UpdateChargeText;
    }

    private void UpdateChargeText(int currentCharge, int maxCharge)
    {
        _view.text = $"{currentCharge} / {maxCharge}";
    }
}
