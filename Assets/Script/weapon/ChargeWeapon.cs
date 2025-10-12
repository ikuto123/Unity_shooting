using UnityEngine;

public class ChargeWeapon : WeaponBaseClass
{
    public ChargeWeapon(int id, string name, int energyCost, int damage ,
        float beamSpeed , float beamLifetime , int maxActiveBeam , float fireRate)  : 
        base(id, name, energyCost,damage , beamSpeed, beamLifetime , maxActiveBeam , fireRate) { }
    

}
