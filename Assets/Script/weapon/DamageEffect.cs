using Beam;
using UnityEngine;

public class DamageEffect : IHitEffect
{
    public void Execute(BeamManager beam, Collision collision, WeaponBaseClass weaponData)
    {
        //衝突相手がダメージを受けるかを判定
        if (collision.gameObject.TryGetComponent<IDamageable>(out var damageableTarget))
        {
            //武器データが持つダメージ量を渡して、ダメージ処理を実行
            damageableTarget.TakeDamage(weaponData.Damage);
        }
        beam.DeActivate();
    }
}
