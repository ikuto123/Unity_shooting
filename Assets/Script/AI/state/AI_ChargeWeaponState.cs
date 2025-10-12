using AIState;
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
        var closestArea = GameManager.Instance.GetClosestRecoveryArea(_ai.MyTeam.Team, _ai.transform.position);
        
        if (closestArea != null)
        {
            // 見つかったエリアを目標に設定
            _targetRecoveryArea = closestArea.transform;
            
            // 回復エリアの位置を目的地として移動を開始
            _ai.Movement.SetTarget(_targetRecoveryArea.position);
            
            Debug.Log("目標の回復エリアを設定: " + _targetRecoveryArea.name);
        }
        else
        {
            Debug.LogWarning("回復エリアが見つかりませんでした。索敵状態に戻ります。");
            // 回復エリアが見つからなかった場合の予備の行動
            _ai.ChangeState(new AI_MoveToTargetAreaState(_ai));
        }
        
        
        Debug.Log("攻撃状態に入りました！");
    }

    public void OnUpdate()
    {
        if (_targetRecoveryArea == null) return;
        
        if (Vector3.Distance(_ai.transform.position, _targetRecoveryArea.position) < arrivalDistance) 
        {
            // キャラクターのエネルギーが最大値に達したら
            if (_ai.CharacterManager.CurrentCharge >= _ai.CharacterManager.MaxCharge)
            {
                Debug.Log("エネルギーが満タンになったので、索敵に戻ります。");
                _ai.ChangeState(new AI_MoveToTargetAreaState(_ai));
            }
        }

    }

    public void OnExit()
    {
        
        _targetRecoveryArea = null;
        Debug.Log("攻撃状態を終了します");
    }
}
