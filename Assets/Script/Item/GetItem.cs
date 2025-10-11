using UnityEngine;

public class GetItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーに接触したかどうかを判定
        if (other.gameObject.TryGetComponent<IRecover>(out var damageableObject))
        {
            Debug.Log(gameObject.name + " を取得した！");

            // --- ここにアイテム取得時の効果を記述 ---
            //damageableObject.Heal(25);
        }
        ItemSpawnManager.Instance.ItemRespawn(gameObject);
    }
}
