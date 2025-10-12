using System;
using System.Collections; 
using UnityEngine;

namespace Beam
{
    public class BeamManager : MonoBehaviour ,ITeamAffiliated
    {
        public int Damage { get; private set; }
        public Team Team { get; private set; }
        
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

        public void Initialize(WeaponBaseClass weaponData,Team team, Action<GameObject> onDeactivatedCallback)
        {
            _weaponData = weaponData;
            this.Team = team;
            // データを設定
            _onDeactivatedCallback = onDeactivatedCallback;

            // 各コンポーネントに初期化を命令
            _mover.Initialize(weaponData.BeamSpeed);
            _lifetime.StartLifetime(weaponData.BeamLifetime);
        }

        public void ExecuteHitEffect(Collider other)
        {
            if (other.gameObject.TryGetComponent<ITeamAffiliated>(out var targetAffiliation))
            {
                if (targetAffiliation.Team == this.Team)
                {
                    return;
                }
            }

            _weaponData?.HitEffect?.Execute(other, _weaponData);
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