// BeamManager.cs

using System;
using UnityEngine;

namespace Beam
{
    public class BeamManager : MonoBehaviour, ITeamAffiliated
    {
        public int Damage { get; private set; }
        public Team Team { get; private set; }
        
        public int BounceCount { get; set; }
        
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

            //反射回数をリセット
            this.BounceCount = 0;

            Mover.Initialize(weaponData.BeamSpeed);
            _lifetime.StartLifetime(weaponData.BeamLifetime);
        }

        //衝突時に呼ばれる
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