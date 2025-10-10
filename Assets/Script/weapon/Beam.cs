using System;
using System.Collections; 
using UnityEngine;

public class Beam : MonoBehaviour
{
    private float _speed;
    private float _lifeTime; 

    private int _damage; 
    private Rigidbody _rigidbody;
    private Action<GameObject> _onDeactivated;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void OnEnable()
    {
        _rigidbody.linearVelocity = transform.forward * _speed;
        
        StartCoroutine(LifetimeCoroutine());
    }
    
    public void Initialize(WeaponBaseClass weaponData, Action<GameObject> onDeactivatedCallback)
    {
        _damage = weaponData.Damage;
        _speed = weaponData.BeamSpeed;
        _lifeTime = weaponData.BeamLifetime;
        
        _onDeactivated = onDeactivatedCallback;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable damageableTarget))
        {
            Debug.Log($"<color=green>HIT!</color> {collision.gameObject.name}に{_damage}ダメージ！");
            damageableTarget.TakeDamage(_damage);
        }
        _onDeactivated?.Invoke(gameObject);
        gameObject.SetActive(false);
    }
    
    private IEnumerator LifetimeCoroutine()
    {
        // 指定した秒数だけ待機する
        yield return new WaitForSeconds(_lifeTime);
        Debug.Log("ビームが寿命で消えました。");
        _onDeactivated?.Invoke(gameObject);
        gameObject.SetActive(false);
    }
}