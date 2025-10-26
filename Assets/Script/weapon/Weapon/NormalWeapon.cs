using UnityEngine;

public class NormalWeapon : WeaponBaseClass
{
    public NormalWeapon(int id, string name, int energyCost, int damage , 
        float beamSpeed, float beamLifetime, int maxActiveBeam , float fireRate)
        : base(id, name, energyCost,damage , beamSpeed, beamLifetime ,maxActiveBeam ,fireRate) { }

    public override bool Fire(IChargeable user)
    {
        bool firedSuccessfully = base.Fire(user);
        return firedSuccessfully;
    }
}
