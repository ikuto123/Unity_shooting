using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("AIの目標エリア_占領エリア")]
    [SerializeField] private Transform[] targetAreas;

    [Header("AIの目標エリア_回復エリア")]
    private Dictionary<Team, List<RecoveryArea>> _recoveryAreasByTeam = new Dictionary<Team, List<RecoveryArea>>();
    
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

    //回復エリアを登録しておく
    public void RegisterRecoveryArea(Team team, RecoveryArea area)
    {
        if (!_recoveryAreasByTeam.ContainsKey(team))
        {
            _recoveryAreasByTeam[team] = new List<RecoveryArea>();
        }
        _recoveryAreasByTeam[team].Add(area);
        Debug.Log($"<color=lightblue>{team}用の回復エリアを登録:</color> {area.gameObject.name}");
    }
    
    //一番近くのエリアを探す
    public RecoveryArea GetClosestRecoveryArea(Team team, Vector3 position)
    {
        if (_recoveryAreasByTeam.ContainsKey(team) && _recoveryAreasByTeam[team].Count > 0)
        {
            return _recoveryAreasByTeam[team]
                .OrderBy(area => Vector3.Distance(area.transform.position, position))
                .FirstOrDefault();
        }
        
        // エリアが見つからなかった場合
        return null;
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
