using System.Collections.Generic;
using UnityEngine;

public class BeamPoolManager : MonoBehaviour
{
    public static BeamPoolManager Instance { get; private set; }
    private List<GameObject> _beamPool;
    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        
        _beamPool = new List<GameObject>();
        
        foreach (Transform child in transform)
        {
            _beamPool.Add(child.gameObject);
            
            child.gameObject.SetActive(false);
        }
    }
    
    public GameObject GetPooledBeam()
    {
        foreach (var beam in _beamPool)
        {
            if (beam != null && !beam.activeInHierarchy)
            {
                return beam;
            }
        }
        
        Debug.LogWarning("プールに利用可能な弾がありません！");
        return null;
    }
}
