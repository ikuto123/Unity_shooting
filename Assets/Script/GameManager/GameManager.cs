using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // --- 各専門マネージャーへの参照 ---
    public CharacterSpawnManager SpawnManager { get; private set; }
    public OccupationManager OccupationManager { get; private set; }
    public RecoveryAreaManager RecoveryManager { get; private set; }

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        // MonoBehaviourである専門家はGetComponentで取得
        SpawnManager = GetComponent<CharacterSpawnManager>();
        OccupationManager = GetComponent<OccupationManager>();
        
        // MonoBehaviourではない専門家はnewでインスタンス化
        RecoveryManager = new RecoveryAreaManager();
    }

    private void Start()
    {
        // 現場監督にキャラクターの配置を指示
        SpawnManager.SpawnCharacters();
    }
}
