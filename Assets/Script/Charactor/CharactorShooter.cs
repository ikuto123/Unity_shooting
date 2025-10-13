using System.Collections.Generic;
using Beam;
using UnityEngine;

public class CharactorShooter : MonoBehaviour
{
    [SerializeField] private Transform _muzzlePoint;
    
    private readonly List<GameObject> _activeBeams = new List<GameObject>();
    private int _poolIndex = 0;
    
    private CharacterManager _characterManager;

    private CharactorTeam _character;
    private void Awake()
    {
        _characterManager = GetComponent<CharacterManager>();
        _character = GetComponent<CharactorTeam>();
    }
    
    private void OnEnable()
    {
        if (_characterManager != null && _characterManager.WeaponManager != null)
        {
            _characterManager.WeaponManager.OnFireRequest += FireBeamObj;
        }
    }

    private void OnDisable()
    {
        if (_characterManager != null && _characterManager.WeaponManager != null)
        {
            _characterManager.WeaponManager.OnFireRequest -= FireBeamObj;
        }
    }

    //弾の発射処理
    private void FireBeamObj(WeaponBaseClass weaponData)
    {
        if (_activeBeams.Count >= weaponData.MaxActiveBeam)
        {
            Debug.Log("発射制限数に達しているため、新しい弾を発射できません。");
            return; 
        }
        _characterManager.UseCharge(weaponData.EnergyCost);
        
        GameObject beamObj = BeamPoolManager.Instance.GetPooledBeam();
        
        if (beamObj != null)
        {
            // エネルギー消費はここで行うのが安全
            _characterManager.UseCharge(weaponData.EnergyCost);
            Debug.Log($"【{weaponData.GunName}】発射成功。");

            ActivateBeam(beamObj, weaponData);
        }
    }
    
    //弾の有効化と初期化
    private void ActivateBeam(GameObject beamObj, WeaponBaseClass weaponData)
    {
        // 発射位置と角度を設定
        beamObj.transform.position = _muzzlePoint.position;
        beamObj.transform.rotation = _muzzlePoint.rotation;

        // 弾を有効化
        beamObj.SetActive(true);
        _activeBeams.Add(beamObj);

        // 弾の初期化（ダメージとコールバックを設定）
        var beamManager = beamObj.GetComponent<BeamManager>();
        if (beamObj != null)
        {
            beamManager.Initialize(weaponData,_character.Team, OnBeamDeactivated);
        }
    }
    
    //弾が非アクティブになった時に呼び出されるコールバックメソッド
    private void OnBeamDeactivated(GameObject projectile)
    {
        if (_activeBeams.Contains(projectile))
        {
            _activeBeams.Remove(projectile);
        }
    }
}
