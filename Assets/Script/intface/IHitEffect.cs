using Beam;
using UnityEngine;

public interface IHitEffect
{
    void Execute(BeamManager beam,Collision collision, WeaponBaseClass weaponData);
}