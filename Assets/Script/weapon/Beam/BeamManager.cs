using System;
using System.Collections; 
using UnityEngine;

namespace Beam
{
    public class BeamManager : MonoBehaviour
    {
        public int Damage { get; private set; }

        private BeamMove _mover;
        private BeamLifeTime _lifetime;
        public BeamCollisionHandler CollisionHandler { get; private set; }

        private Action<GameObject> _onDeactivatedCallback;

        private WeaponBaseClass _weaponData;
        private void Awake()
        {
            _mover = GetComponent<BeamMove>();
            _lifetime = GetComponent<BeamLifeTime>();
            CollisionHandler = GetComponent<BeamCollisionHandler>();
        }

        public void Initialize(WeaponBaseClass weaponData, Action<GameObject> onDeactivatedCallback)
        {
            _weaponData = weaponData;
            // データを設定
            _onDeactivatedCallback = onDeactivatedCallback;

            // 各コンポーネントに初期化を命令
            _mover.Initialize(weaponData.BeamSpeed);
            _lifetime.StartLifetime(weaponData.BeamLifetime);
        }

        public void ExecuteHitEffect(Collision collision)
        {
            // 保持している武器データから効果を取り出し、実行する
            _weaponData?.HitEffect?.Execute(collision, _weaponData);

            DeActivate();
        }
        
        // 衝突時や寿命で呼ばれる非アクティブ化メソッド
        public void DeActivate()
        {
            if (!gameObject.activeSelf) return; // 既に非アクティブなら何もしない

            _onDeactivatedCallback?.Invoke(gameObject);
            gameObject.SetActive(false);
        }
    }
}