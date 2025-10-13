using UnityEngine;

public class CharactorTeam : MonoBehaviour , ITeamAffiliated
{
    [SerializeField] private Team _team;
    public Team Team => _team;

    //チームの初期化
    public void Initialize(Team assignedTeam)
    {
        this._team = assignedTeam;
    }
}
