using Beam;
using UnityEngine;

public class ReflectionEffect : IHitEffect
{
    private readonly int _wallLayer;
    
    public ReflectionEffect()
    {
        _wallLayer = LayerMask.NameToLayer("Wall");
    }
    public void Execute(BeamManager beam, Collision collision, WeaponBaseClass weaponData)
    {
        //既に一度反射している場合は、何に当たっても消滅させる
        if (beam.BounceCount > 0)
        {
            beam.DeActivate();
            return;
        }
        
        //衝突したオブジェクトが壁かどうかをレイヤーで判定
        if (collision.gameObject.layer == _wallLayer)
        {
            beam.BounceCount++; 

            Vector3 inDirection = beam.transform.forward;
            Vector3 inNormal = collision.contacts[0].normal;
            Vector3 reflectVector = Vector3.Reflect(inDirection, inNormal);

            beam.transform.rotation = Quaternion.LookRotation(reflectVector);
            beam.Mover.UpdateVelocity();
            
        }
        else//壁でないオブジェクトに当たった場合は、反射せずに消滅
        {
            beam.DeActivate();
        }
    }    
}
