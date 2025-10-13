using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponManager
{
    public event Action<int, string> OnGunSwitched; 
    public event Action<WeaponBaseClass> OnFireRequest;

    private readonly Dictionary<int, IWeapon> _guns = new Dictionary<int, IWeapon>();
    private readonly List<int> _gunIDs = new List<int>();
    private int _currentGunID;
    
    public int GunID => _currentGunID;
    
    public IWeapon CurrentGun
    {
        get
        {
            if (_guns.TryGetValue(_currentGunID, out var gun))
            {
                return gun;
            }
            return null;
        }
    }

    // コンストラクタをpublicに変更
    public WeaponManager()
    {
        InitializeGuns();
        if (_guns.Count > 0)
        {
            // デフォルトで最初の武器を選択
            SelectGun(_gunIDs[0]);
        }
    }
    
    private void InitializeGuns()
    {
        var jsonTextAsset = Resources.Load<TextAsset>("Data/WeaponsData");
        if (jsonTextAsset == null)
        {
            Debug.LogError("Weapon data file not found at 'Resources/Data/WeaponsData'");
            return;
        }
        
        var weaponList = JsonUtility.FromJson<WeaponList>(jsonTextAsset.text);
        
        foreach (var weaponData in weaponList.weapons)
        {
            Type weaponType = Type.GetType(weaponData.className);

            if (weaponType != null && typeof(IWeapon).IsAssignableFrom(weaponType))
            {
                try
                {
                    object[] args =
                    {
                        weaponData.id, 
                        weaponData.gunName, 
                        weaponData.energyCost ,
                        weaponData.damage,
                        weaponData.beamSpeed,
                        weaponData.beamLifetime,
                        weaponData.maxActiveBeam,
                        weaponData.fireRate
                    };
                    IWeapon weaponInstance = (IWeapon)Activator.CreateInstance(weaponType, args);
                    _guns.Add(weaponData.id, weaponInstance);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to create instance of {weaponData.className}: {e.Message}");
                }
            }
            else
            {
                Debug.LogError($"Invalid or not found weapon class name: {weaponData.className}");
            }
        }
        _gunIDs.AddRange(_guns.Keys.OrderBy(id => id));
    }

    public void SelectGun(int gunID)
    {
        if (_gunIDs.Contains(gunID))
        {
            _currentGunID = gunID;
            var gun = CurrentGun;
            //Debug.Log($"ID {gunID} の {gun.GunName} を選択しました。");
            OnGunSwitched?.Invoke(_currentGunID, gun.GunName);
        }
    }

    public void FireCurrentGun(IChargeable user)
    {
        var gun = CurrentGun;
        if (gun != null)
        {
            if (gun.Fire(user))
            {
                var weaponBase = gun as WeaponBaseClass;
                OnFireRequest?.Invoke(weaponBase);
            }
        }
        else
        {
            Debug.Log("装備している銃がありません。");
        }
    }
}