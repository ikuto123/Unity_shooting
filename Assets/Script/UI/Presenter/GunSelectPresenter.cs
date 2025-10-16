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

    private void UpdateGunSelectUI(int currentGunId, string maxCharge)
    {
        int idx = currentGunId - 1;
        if (idx == _selectedIndex) return;
        
        if (_selectedIndex >= 0 && _selectedIndex < _gunImageOutlines.Length)
        {
            var prev = _gunImageOutlines[_selectedIndex];
            if (prev) prev.enabled = false; 
        }
        
        if (idx >= 0 && idx < _gunImageOutlines.Length)
        {
            var now = _gunImageOutlines[idx];
            if (now)
            {
                now.enabled = true;
            }
        }

        _selectedIndex = idx;
    }
}
