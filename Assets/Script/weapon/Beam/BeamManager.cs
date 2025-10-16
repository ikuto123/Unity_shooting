// BeamManager.cs

using System;
using UnityEngine;

namespace Beam
{
    public class BeamManager : MonoBehaviour, ITeamAffiliated
    {
        public int Damage { get; private set; }
        public Team Team { get; private set; }
        
        // 反射回数をカウントするプロパティ
        public int BounceCount { get; set; }
        
        // 外部からBeamMoveにアクセスできるようにプロパティを追加
        public BeamMove Mover { get; private set; }
        private BeamLifeTime _lifetime;
        
        private Action<GameObject> _onDeactivatedCallback;
        private WeaponBaseClass _weaponData;

        private void Awake()
        {
            Mover = GetComponent<BeamMove>();
            _lifetime = GetComponent<BeamLifeTime>();
        }

        public void Initialize(WeaponBaseClass weaponData, Team team, Action<GameObject> onDeactivatedCallback)
        {
            _weaponData = weaponData;
            this.Team = team;
            _onDeactivatedCallback = onDeactivatedCallback;

            // 初期化時に反射回数をリセット
            this.BounceCount = 0;

            Mover.Initialize(weaponData.BeamSpeed);
            _lifetime.StartLifetime(weaponData.BeamLifetime);
        }

        // 衝突時に呼ばれる新しいメソッド
        public void OnCollision(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<ITeamAffiliated>(out var targetAffiliation))
            {
                if (targetAffiliation.Team == this.Team)
                {
                    DeActivate();
                    return; 
                }
            }

            // 衝突時の振る舞いを、武器が持つHitEffectに完全に委譲する
            _weaponData?.HitEffect?.Execute(this, collision, _weaponData);
        }

        public void DeActivate()
        {
            if (!gameObject.activeSelf) return;

            _onDeactivatedCallback?.Invoke(gameObject);
            gameObject.SetActive(false);
        }
    }
}