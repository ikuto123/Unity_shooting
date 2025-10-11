using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("AIの目標エリア")]
    [SerializeField] private Transform[] targetAreas;

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
    }


    // AIの目標エリアの座標を取得します。
    public Vector3 GetTargetAreaPosition() 
    {
        if (targetAreas == null || targetAreas.Length == 0)
        {
            Debug.LogError("GameManagerに目標エリアが設定されていません！");
            return Vector3.zero;
        }

        // 設定されたエリアの中からランダムに1つを返す
        return targetAreas[Random.Range(0, targetAreas.Length)].position;
    }
}
