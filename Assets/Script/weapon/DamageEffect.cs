using UnityEngine;

public class DamageEffect : IHitEffect
{
    public void Execute(Collision collision, WeaponBaseClass weaponData)
    {
        // 衝突相手からIDamageableコンポーネントを探す
        if (collision.gameObject.TryGetComponent<IDamageable>(out var damageableTarget))
        {
            // 武器データが持つダメージ量を渡して、ダメージ処理を実行
            damageableTarget.TakeDamage(weaponData.Damage);
            Debug.Log($"<color=green>HIT!</color> {collision.gameObject.name}に{weaponData.Damage}ダメージ！");
        }
    }
}
