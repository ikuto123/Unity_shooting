using AIState;
using GameScene;
using UnityEngine;

public class AI_ChargeWeaponState : IState
{
    private readonly AIController _ai;
    private Transform _targetRecoveryArea;

    private float arrivalDistance = 2.0f;
    public AI_ChargeWeaponState(AIController ai)
    {
        _ai = ai;
    }
    
    public void OnEnter()
    {
        var closestArea = GameManager.Instance.RecoveryManager.GetClosestRecoveryArea(_ai.MyTeam.Team, _ai.transform.position);
        
        if (closestArea != null)
        {
            //見つかった回復エリアを目標に設定
            _targetRecoveryArea = closestArea.transform;
            
            _ai.Movement.SetTarget(_targetRecoveryArea.position);
            
        }
        else
        {
            //回復エリアが見つからなかった場合は索敵に戻る
            _ai.ChangeState(new AI_MoveToTargetAreaState(_ai));
        }
    }

    public void OnUpdate()
    {
        if (_targetRecoveryArea == null) return;
        
        //エネルギーが最大値に達した場合の行動
        if (_ai.CharacterManager.CurrentCharge >= _ai.CharacterManager.MaxCharge)
        {
            _ai.ChangeState(new AI_MoveToTargetAreaState(_ai));
        }
        

    }

    public void OnExit()
    {
        _targetRecoveryArea = null;
    }
}
