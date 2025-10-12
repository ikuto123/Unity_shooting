using UnityEngine;

public class CharactorTeam : MonoBehaviour , ITeamAffiliated
{
    [Tooltip("所属するチームの選択")]
    [SerializeField] private Team team;
    public Team Team => team;
}
