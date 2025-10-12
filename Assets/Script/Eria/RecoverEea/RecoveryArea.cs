using System;
using UnityEngine;

public class RecoveryArea : MonoBehaviour
{
    private int HPamount = 1;
    private int Chargeamount = 1;
    
    [Header("回復設定")]
    [SerializeField] private Team _targetTeam;

    private void Start()
    {
        GameManager.Instance.RegisterRecoveryArea(_targetTeam, this);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<ITeamAffiliated>(out var teamAffiliation) &&
            teamAffiliation.Team == _targetTeam &&
            other.gameObject.TryGetComponent<IRecover>(out var recoverTarget))
        {
            Debug.Log("Recovery");
            recoverTarget.RecoverHp(HPamount);
            recoverTarget.RecoverCharge(Chargeamount);
        }
    }
    
}
