using UnityEngine;

public interface IHitEffect
{
    void Execute(Collider hitCollider, WeaponBaseClass weaponData);
}