using System;
using UnityEngine;

public class WeaponSkin : MonoBehaviour
{
    private int _currentGunID = -1;

    [SerializeField] private GameObject[] _GunSkins;

    private CharacterManager _characterManager;
    private void Start()
    {
        _characterManager = GetComponentInParent<CharacterManager>();
        _characterManager.WeaponManager.OnGunSwitched += HandleGunSkinSwitch;
    }
    
    //銃のスキンの変更
    private void HandleGunSkinSwitch(int gunID, string gunName)
    {
        foreach (GameObject skin in _GunSkins) skin.SetActive(false);
        _GunSkins[gunID - 1].SetActive(true);
    }
    
    private void OnDestroy()
    {
        if (_characterManager != null && _characterManager.WeaponManager != null)
        {
            _characterManager.WeaponManager.OnGunSwitched -= HandleGunSkinSwitch;
        }
    }
}
