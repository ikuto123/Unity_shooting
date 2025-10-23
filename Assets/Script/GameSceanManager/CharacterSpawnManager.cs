using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSpawnManager : MonoBehaviour
{
   public static CharacterSpawnManager Instance { get; private set; }

    [Header("キャラクターリスト")]
    [SerializeField] private List<GameObject> _charactersList;
    
    private float _spawnHeightOffset = 2f;
    private float _spawnAreaInset = 1.0f;
    
    private Dictionary<GameObject, Transform> _assignedSpawnPoints = new Dictionary<GameObject, Transform>();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    // 初期スポーン
    public void SpawnCharacters()
    {
        var spawnPointsByTeam = FindObjectsOfType<TeamSpawnPoint>()
            .GroupBy(sp => sp.team)
            .ToDictionary(g => g.Key, g => g.Select(sp => sp.transform).ToList());
        
        if (!spawnPointsByTeam.TryGetValue(Team.A, out var teamASpawnPoints) || teamASpawnPoints.Count == 0 ||
            !spawnPointsByTeam.TryGetValue(Team.B, out var teamBSpawnPoints) || teamBSpawnPoints.Count == 0)
        {
            Debug.LogError("各チームに最低1つのスポーン地点が設定されていません！");
            return;
        }
        
        int teamACount = 0, teamBCount = 0;

        for (int i = 0; i < _charactersList.Count; i++)
        {
            GameObject character = _charactersList[i];
            Transform spawnArea = null;

            if (i % 2 == 0) // TeamA
            {
                character.GetComponent<CharactorTeam>().Initialize(Team.A);
                spawnArea = teamASpawnPoints[teamACount % teamASpawnPoints.Count];
                teamACount++;
            }
            else // TeamB
            {
                character.GetComponent<CharactorTeam>().Initialize(Team.B);
                spawnArea = teamBSpawnPoints[teamBCount % teamBSpawnPoints.Count];
                teamBCount++;
            }

            if (spawnArea != null)
            {
                //エリア内のランダムな位置を取得して配置 
                character.transform.position = GetRandomPosition(spawnArea);
                character.transform.rotation = spawnArea.rotation;
                
                _assignedSpawnPoints[character] = spawnArea;
            }
            
            if (!character.activeSelf) character.SetActive(true);
        }
    }

    // リスポーン要求を受け付ける
    public void RequestRespawn(CharacterManager character)
    {
        StartCoroutine(RespawnCoroutine(character));
    }

    // リスポーン処理
    private IEnumerator RespawnCoroutine(CharacterManager character)
    {
        yield return new WaitForSeconds(character.RespawnDelay);

        if (_assignedSpawnPoints.TryGetValue(character.gameObject, out Transform spawnArea))
        {
            //エリア内のランダムな位置を取得して再配置 
            character.transform.position = GetRandomPosition(spawnArea);
            character.transform.rotation = spawnArea.rotation;

            character.ResetStatus();
            character.gameObject.SetActive(true);
            
            Debug.Log($"<color=lime>{character.name} をリスポーンさせました。</color>");
        }
    }
    
    //スポーン位置調整
    private Vector3 GetRandomPosition(Transform area)
    {
        Vector3 center = area.position;
        Vector3 scale = area.localScale;
        
        float effectiveScaleX = Mathf.Max(0, scale.x - (_spawnAreaInset * 2));
        float effectiveScaleZ = Mathf.Max(0, scale.z - (_spawnAreaInset * 2));
        
        float randomX = center.x + Random.Range(-effectiveScaleX / 2, effectiveScaleX / 2);
        float randomZ = center.z + Random.Range(-effectiveScaleZ / 2, effectiveScaleZ / 2);
        
        float yPosition = center.y + _spawnHeightOffset;

        return new Vector3(randomX, yPosition, randomZ);
   }
}
