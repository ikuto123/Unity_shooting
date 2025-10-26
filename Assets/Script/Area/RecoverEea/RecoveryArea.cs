using System;
using GameScene;
using UnityEngine;

public class RecoveryArea : MonoBehaviour
{
    private int _hpAmount = 1;
    private int _chargeAmount = 1;
    
    [Header("回復設定")]
    [SerializeField] private Team _targetTeam;

    private void Start()
    {
        GameManager.Instance.RecoveryManager.RegisterRecoveryArea(_targetTeam, this);
    }

    //キャラクターのチームを判定して回復
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<ITeamAffiliated>(out var teamAffiliation) &&
            teamAffiliation.Team == _targetTeam &&
            other.gameObject.TryGetComponent<IRecover>(out var recoverTarget))
        {
            recoverTarget.RecoverHp(_hpAmount);
            recoverTarget.RecoverCharge(_chargeAmount);
        }
    }
    
}
