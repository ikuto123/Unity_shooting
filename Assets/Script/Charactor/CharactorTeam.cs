using UnityEngine;

public class CharactorTeam : MonoBehaviour
{
    [Tooltip("所属するチームの選択")]
    [SerializeField] private Team team;
    public Team Team => team;
}
