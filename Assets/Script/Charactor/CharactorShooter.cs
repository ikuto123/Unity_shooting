using System.Collections.Generic;
using Beam;
using GameScene;
using UnityEngine;

public class CharactorShooter : MonoBehaviour
{
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private SoundData _fireSound;
    
    private readonly List<GameObject> _activeBeams = new List<GameObject>();
    private int _poolIndex = 0;
    
    private CharacterManager _characterManager;

    private CharactorTeam _character;
    private void Awake()
    {
        _characterManager = GetComponent<CharacterManager>();
        _character = GetComponent<CharactorTeam>();
    }
    
    private void Start()
    {
        if (_characterManager?.WeaponManager != null)
        {
            _characterManager.WeaponManager.OnFireRequest -= FireBeamObj;
            _characterManager.WeaponManager.OnFireRequest += FireBeamObj;
        }
        else
        {
            Debug.LogError($"{gameObject.name}: WeaponManagerが見つかりません！", this);
        }
    }

    private void OnEnable()
    {
        if (_characterManager?.WeaponManager != null)
        {
            _characterManager.WeaponManager.OnFireRequest -= FireBeamObj;
            
            _characterManager.WeaponManager.OnFireRequest += FireBeamObj;
        }
        else
        {
            Debug.LogError($"{gameObject.name}: WeaponManagerが見つかりません！", this);
        }
    }
    
    private void OnDisable()
    {
        if (_characterManager != null && _characterManager.WeaponManager != null)
        {
            _characterManager.WeaponManager.OnFireRequest -= FireBeamObj;
        }
        _activeBeams.Clear();
    }

    //弾の発射処理
    private void FireBeamObj(WeaponBaseClass weaponData)
    {
        if (!GameManager.Instance.IsGameActive)
        {
            return;
        }
        
        if (_activeBeams.Count >= weaponData.MaxActiveBeam)
        {
            Debug.Log("発射制限数に達しているため、新しい弾を発射できません。");
            return; 
        }
        
        GameObject beamObj = BeamPoolManager.Instance.GetPooledBeam();
        
        if (beamObj != null)
        {
            //エネルギー消費
            _characterManager.UseCharge(weaponData.EnergyCost);
            
            SoundManager.Instance.PlaySE_3D(_fireSound, transform.position);
            
            ActivateBeam(beamObj, weaponData);
        }
        else
        {
            Debug.LogError("弾のプールから取得できませんでした。PoolSizeを増やしてください");
        }
    }
    
    //弾の有効化と初期化
    private void ActivateBeam(GameObject beamObj, WeaponBaseClass weaponData)
    {
        //発射位置と角度を設定
        beamObj.transform.position = _muzzlePoint.position;
        beamObj.transform.rotation = _muzzlePoint.rotation;

        //弾を有効化
        beamObj.SetActive(true);
        _activeBeams.Add(beamObj);

        //弾の初期化
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
