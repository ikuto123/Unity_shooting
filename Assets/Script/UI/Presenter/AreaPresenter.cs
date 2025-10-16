using UnityEngine;
using UnityEngine.UI;

public class AreaPresenter
{
    private readonly AreaControl _areaControl;
    
    // View
    private readonly Image _teamASlider;
    private readonly Image _teamBSlider;

    public AreaPresenter(AreaControl areaControl, Image teamASlider, Image teamBSlider)
    {
        _areaControl = areaControl;
        _teamASlider = teamASlider;
        _teamBSlider = teamBSlider;
        
        // スライダーの最大値を設定
        _teamASlider.fillAmount = _areaControl.TimeToWin;
        _teamBSlider.fillAmount = _areaControl.TimeToWin;
    }
    
    public void Enable()
    {
        _areaControl.OnProgressUpdated += UpdateTeamSliders;
        UpdateTeamSliders(0,0);
    }

    public void Disable()
    {
        _areaControl.OnProgressUpdated -= UpdateTeamSliders;
    }
    
    private void UpdateTeamSliders(float teamAProgress, float teamBProgress)
    {
        _teamASlider.fillAmount = teamAProgress / _areaControl.TimeToWin;
        _teamBSlider.fillAmount = teamBProgress / _areaControl.TimeToWin;
    }
}
