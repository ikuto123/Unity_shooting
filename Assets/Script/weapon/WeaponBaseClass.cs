using System;
using UnityEngine;

public abstract class WeaponBaseClass: IWeapon
{
    public int GunID { get; private set; }
    public string GunName { get; private set; }
    public int EnergyCost { get; private set; }
    public int Damage { get; private set; }
    public float BeamSpeed { get; private set; }  
    public float BeamLifetime { get; private set; }
    public IHitEffect HitEffect { get; protected set; }
    public float FireRate { get; private set; }
    public int MaxActiveBeam { get; private set; }
    
    private float _nextFireTime = 0f;
    public WeaponBaseClass(int id, string name, int energyCost, 
        int damage, float beamSpeed, float beamLifetime, int maxActiveBeam,float fireRate)
    {
        this.GunID = id;
        this.GunName = name;
        this.EnergyCost = energyCost;
        this.Damage = damage;
        this.BeamSpeed = beamSpeed;
        this.BeamLifetime = beamLifetime;
        this.MaxActiveBeam = maxActiveBeam;
        this.HitEffect = new DamageEffect();
        this.FireRate = fireRate;
    }

    public virtual bool Fire(IChargeable user)
    {
        //発射感覚の調整
        if (Time.time < _nextFireTime)
        {
            return false; 
        }
        
        if (user != null && user.HasEnoughCharge(EnergyCost))//エネルギーがあれば打つ
        {
            if (FireRate > 0)//次のクールタイムを測定
            {
                _nextFireTime = Time.time + 1f / FireRate;
            }
            user.UseCharge(EnergyCost);
            
            Debug.Log($"【{GunName}】発射成功。残りチャージ: {user.CurrentCharge} / {user.MaxCharge}");
            Debug.Log("ダメージ数" + Damage);
            return true;
        }
        else
        {
            Debug.Log($"{GunName}はチャージ不足で発射できません。");
            return false;
        }
    }
    
}
