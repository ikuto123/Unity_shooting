using UnityEngine;

public interface IHitEffect
{
    void Execute(Collision collision, WeaponBaseClass weaponData);
}