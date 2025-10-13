using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RecoveryAreaManager
{
    private Dictionary<Team, List<RecoveryArea>> _recoveryAreasByTeam = new Dictionary<Team, List<RecoveryArea>>();

    public void RegisterRecoveryArea(Team team, RecoveryArea area)
    {
        if (!_recoveryAreasByTeam.ContainsKey(team))
        {
            _recoveryAreasByTeam[team] = new List<RecoveryArea>();
        }
        _recoveryAreasByTeam[team].Add(area);
    }

    public RecoveryArea GetClosestRecoveryArea(Team team, Vector3 position)
    {
        if (_recoveryAreasByTeam.ContainsKey(team) && _recoveryAreasByTeam[team].Count > 0)
        {
            return _recoveryAreasByTeam[team]
                .OrderBy(area => Vector3.Distance(area.transform.position, position))
                .FirstOrDefault();
        }
        return null;
    }
}