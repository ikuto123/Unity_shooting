using UnityEngine;

public class OccupationManager : MonoBehaviour
{
    [Header("AIの目標エリア_占領エリア")]
    [SerializeField] private Transform[] _targetAreas;

    public Vector3 GetTargetAreaPosition()
    {
        if (_targetAreas == null || _targetAreas.Length == 0)
        {
            Debug.LogError("ObjectiveManagerに目標エリアが設定されていません！");
            return Vector3.zero;
        }
        return _targetAreas[Random.Range(0, _targetAreas.Length)].position;
    }
}
