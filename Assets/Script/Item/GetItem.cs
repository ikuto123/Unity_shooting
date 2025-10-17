using UnityEngine;

public enum Itemtype
{
    hart,
    charge,
}
public class GetItem : MonoBehaviour
{
    [Header("アイテムの種類")]
    [SerializeField] private Itemtype _type;
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーに接触したかどうかを判定
        if (other.gameObject.TryGetComponent<IRecover>(out var damageableObject))
        {
            Debug.Log(gameObject.name + " を取得した！");

            // --- ここにアイテム取得時の効果を記述 ---

            if (this._type == Itemtype.hart)
            {
                Debug.Log("回復取得");
                damageableObject.RecoverHp(25);
            }
            
            else if (this._type == Itemtype.charge)
            {
                Debug.Log("チャージ取得");
                damageableObject.RecoverCharge(25);
            }
        }
        ItemSpawnManager.Instance.ItemRespawn(gameObject);
    }
}
