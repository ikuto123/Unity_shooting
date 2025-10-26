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
    
    public WeaponManager()
    {
        InitializeGuns();
        if (_guns.Count > 0)
        {
            //初期は最初の武器を選択
            SelectGun(_gunIDs[0]);
        }
    }
    
    //Jsonから銃のデータを読み込み
    private void InitializeGuns()
    {
        var jsonTextAsset = Resources.Load<TextAsset>("Data/WeaponsData");
        if (jsonTextAsset == null)
        {
            Debug.LogError("データがありません'Resources/Data/WeaponsData'");
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
                    Debug.LogError($"インスタンスの作成に失敗しました {weaponData.className}: {e.Message}");
                }
            }
            else
            {
                Debug.LogError($"武器クラス名が見つかりません: {weaponData.className}");
            }
        }
        _gunIDs.AddRange(_guns.Keys.OrderBy(id => id));
    }

    public List<int> GetWeaponIDs() { return _gunIDs; }
    
    //どの銃を選択しているのか
    public void SelectGun(int gunID)
    {
        if (_gunIDs.Contains(gunID))
        {
            _currentGunID = gunID;
            var gun = CurrentGun;
            OnGunSwitched?.Invoke(_currentGunID, gun.GunName);
        }
    }

    //現在の銃を発射する
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
            Debug.LogError("装備している銃がありません。");
        }
    }
}