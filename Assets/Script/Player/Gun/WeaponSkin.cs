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
        //デリゲート関数の登録
        _characterManager.WeaponManager.OnGunSwitched += HandleGunSkinSwitch;
    }

    //デリゲート関数(イベントのみで呼ばれる)
    private void HandleGunSkinSwitch(int gunID, string gunName)
    {
        foreach (GameObject skin in _GunSkins) skin.SetActive(false);
        _GunSkins[gunID - 1].SetActive(true);
    }
    
    
    //このオブジェクトが消えた際に登録を解除する
    private void OnDestroy()
    {
        if (_characterManager != null && _characterManager.WeaponManager != null)
        {
            _characterManager.WeaponManager.OnGunSwitched -= HandleGunSkinSwitch;
        }
    }
}
