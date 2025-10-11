using System.Collections.Generic;
using UnityEngine;

public class CharactorShooter : MonoBehaviour
{
    [SerializeField] private GameObject[] _beamPool;
    
    private int _maxActiveProjectiles = 5;
    
    private readonly List<GameObject> _activeProjectiles = new List<GameObject>();
    private int _poolIndex = 0;
    
    private CharacterManager _characterManager;

    private void Awake()
    {
        _characterManager = GetComponent<CharacterManager>();
        foreach (var projectile in _beamPool)
        {
            projectile.SetActive(false);
        }
    }

    void Start()
    {
        if (_characterManager != null && _characterManager.WeaponManager != null)
        {
            _characterManager.WeaponManager.OnFireRequest += FireProjectile;
        }
        else
        {
            Debug.LogError("CharacterManager または WeaponInventory が見つからないため、イベントを登録できませんでした。", this.gameObject);
        }
    }
    
    private void OnEnable()
    {
        if (_characterManager != null && _characterManager.WeaponManager != null)
        {
            _characterManager.WeaponManager.OnFireRequest += FireProjectile;
        }
    }

    private void OnDisable()
    {
        if (_characterManager != null && _characterManager.WeaponManager != null)
        {
            _characterManager.WeaponManager.OnFireRequest -= FireProjectile;
        }
    }

    private void FireProjectile(WeaponBaseClass weaponData)
    {
        if (_activeProjectiles.Count >= _maxActiveProjectiles)
        {
            Debug.Log("発射制限数に達しているため、新しい弾を発射できません。");
            return; 
        }
        
        for (int i = 0; i < _beamPool.Length; i++)
        {
            int index = (_poolIndex + i) % _beamPool.Length;
            GameObject pooledObject = _beamPool[index];

            if (!pooledObject.activeInHierarchy)
            {
                // 3. 利用可能な弾を見つけたら発射処理を行う
                ActivateProjectile(pooledObject, weaponData);

                // 4. 次回検索時のためにインデックスを更新
                _poolIndex = (index + 1) % _beamPool.Length;
                
                return; // 発射したのでメソッドを抜ける
            }
        }
    }
    
    //弾の有効化と初期化
    private void ActivateProjectile(GameObject beamObj, WeaponBaseClass weaponData)
    {
        // 発射位置と角度を設定
        //beam.transform.position = _muzzlePoint.position;
        //beam.transform.rotation = _muzzlePoint.rotation;

        // 弾を有効化
        beamObj.SetActive(true);
        _activeProjectiles.Add(beamObj);

        // 弾の初期化（ダメージとコールバックを設定）
        var beamClass = beamObj.GetComponent<Beam>();
        if (beamObj != null)
        {
            beamClass.Initialize(weaponData, OnProjectileDeactivated);
        }
    }
    
    //弾が非アクティブになった時に呼び出されるコールバックメソッド
    private void OnProjectileDeactivated(GameObject projectile)
    {
        // アクティブリストから削除する
        if (_activeProjectiles.Contains(projectile))
        {
            _activeProjectiles.Remove(projectile);
        }
    }
}
