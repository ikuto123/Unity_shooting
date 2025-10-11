using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private  GameObject _itemPrefab;
    
    [SerializeField] private string _prefabResourcePath; // Resourcesフォルダ内のパス
    
    //nullチェックとResourcesからの読み込みを行うプロパティ
    public GameObject ItemPrefab
    {
        get
        {
            if (_itemPrefab == null)
            {
                Debug.LogWarning($"ItemData ({this.name}): Inspectorの参照がnullです。Resourcesから'{_prefabResourcePath}'を読み込みます。");
                
                _itemPrefab = Resources.Load<GameObject>(_prefabResourcePath);
                
                if (_itemPrefab == null)
                {
                    Debug.LogError($"ItemData ({this.name}): Resourcesフォルダにも'{_prefabResourcePath}'が見つかりませんでした！");
                }
            }
            // 最終的に_itemPrefabを返す
            return _itemPrefab;
        }
    }
}
