using UnityEngine;

public class ReflectionWeapon : WeaponBaseClass
{
    public ReflectionWeapon(int id, string name, int energyCost ,int damage,
        float beamSpeed, float beamLifetime , int maxActiveBeam , float fireRate) 
        : base(id, name, energyCost,damage , beamSpeed, beamLifetime , maxActiveBeam, fireRate)
    {
        this.HitEffect = new ReflectionEffect();
    }
    
    public override bool Fire(IChargeable user)
    {
        bool firedSuccessfully = base.Fire(user);
        return firedSuccessfully;
    }
}
