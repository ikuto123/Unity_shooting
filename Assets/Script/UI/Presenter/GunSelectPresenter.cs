using UnityEngine;
using UnityEngine.UI;

public class GunSelectPresenter
{
    private readonly CharacterManager _model;
    private readonly GameObject _view;

    private Outline[] _gunImageOutlines;
    public GunSelectPresenter(CharacterManager model, GameObject view)
    {
        _model = model;
        _view = view;
        
        int childCount = _view.transform.childCount;
        _gunImageOutlines = new Outline[childCount];
        for(int i = 0; i < childCount; i++)
        {
            var child = _view.transform.GetChild(i);
            if (child.TryGetComponent<Outline>(out var outline))
            {
                _gunImageOutlines[i] = outline;
                outline.enabled = false; 
            }
            else
            {
                Debug.LogError("コンポーネントがありません");
            }
        }
    }
    
    public void Enable()
    {
        _model.WeaponManager.OnGunSwitched += UpdateGunSelectUI;
        UpdateGunSelectUI(_model.WeaponManager.GunID, _model.name);
    }

    public void Disable()
    {
        _model.WeaponManager.OnGunSwitched += UpdateGunSelectUI;
    }
    
    private int _selectedIndex = -1;

    //現在の武器に応じて武器のUIにアウトラインを付ける
    private void UpdateGunSelectUI(int currentGunId, string maxCharge)
    {
        int idx = currentGunId - 1;
        if (idx == _selectedIndex) return;
        
        if (_selectedIndex >= 0 && _selectedIndex < _gunImageOutlines.Length)
        {
            var prevOutline = _gunImageOutlines[_selectedIndex];
            if (prevOutline) prevOutline.enabled = false; 
        }
        
        if (idx >= 0 && idx < _gunImageOutlines.Length)
        {
            var nowOutline = _gunImageOutlines[idx];
            if (nowOutline)
            {
                nowOutline.enabled = true;
            }
        }

        _selectedIndex = idx;
    }
}
