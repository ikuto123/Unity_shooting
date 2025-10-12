using UnityEngine;

public class ReflectionWeapon : WeaponBaseClass
{
    private string _GunName;

    public ReflectionWeapon(int id, string name, int energyCost ,int damage,
        float beamSpeed, float beamLifetime , int maxActiveBeam , float fireRate) 
        : base(id, name, energyCost,damage , beamSpeed, beamLifetime , maxActiveBeam, fireRate)
    {
        
    }
}
