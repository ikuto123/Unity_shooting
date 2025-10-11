using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
  public static ItemSpawnManager Instance { get; private set; }
  
    private int _poolSize = 10;

    [Header("生成する場所")]
    [SerializeField] private BoxCollider _spawnArea;

    [Header("アイテムデータ(スクリプタブルオブジェクト)")]
    [SerializeField] private List<ItemData> _possibleItems;
    
    [Header("ヒエラルキー上での親オブジェクト")]
    [SerializeField] private Transform _pearentTransform;
    
    private int _initialSpawnCount = 10;
    
    [Header("アイテムが取得されてから再生成されるまでの時間（秒）")]
    private float _respawnDelay = 5f;

    // 生成した全てのアイテムを保管しておくプール
    private List<GameObject> _itemPool = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        InitializePool();
    }

    private void Start()
    {
        SpawnInitialItems();
    }
    
    //プールに必要なアイテムを全て生成し、非アクティブ状態で保管する
    private void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            //アイテムの種類が均等になるようにプールを作成
            ItemData itemToPool = _possibleItems[i % _possibleItems.Count];
            GameObject itemInstance = Instantiate(itemToPool.itemPrefab, Vector3.zero, Quaternion.identity, _pearentTransform);
            
            itemInstance.SetActive(false);
            _itemPool.Add(itemInstance);
        }
    }

    //プールから初期数のアイテムを取り出して配置する
    private void SpawnInitialItems()
    {
        int spawnCount = Mathf.Min(_initialSpawnCount, _poolSize);
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnFromPool();
        }
    }
    
    //プールから非アクティブなアイテムを1つ見つけて配置する
    private void SpawnFromPool()
    {
        foreach (var item in _itemPool)
        {
            if (!item.activeInHierarchy)
            {
                item.transform.position = GetRandomPositionInArea();
                item.SetActive(true);
                return; 
            }
        }
        Debug.LogWarning("プールに利用可能なアイテムがありません！");
    }
    
    
    public void ItemRespawn(GameObject item)
    {
        item.SetActive(false); 
        StartCoroutine(ItemRespawnTimer());
    }


    //一定時間後にプールから新しいアイテムを配置する
    private IEnumerator ItemRespawnTimer()
    {
        yield return new WaitForSeconds(_respawnDelay);
        SpawnFromPool();
    }


    //スポーン範囲内のランダムな座標を返す
    private Vector3 GetRandomPositionInArea()
    {
        Bounds bounds = _spawnArea.bounds;
        float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float randomY = bounds.center.y + 1f; 
        float randomZ = UnityEngine.Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(randomX, randomY, randomZ);
    }
}
