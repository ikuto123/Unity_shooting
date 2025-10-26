using AIState;
using UnityEngine;

public class AI_ChaseEnemyState : IState
{
    private readonly AIController _ai;

    public AI_ChaseEnemyState(AIController ai)
    {
        _ai = ai;
    }

    public void OnEnter()
    {
        if (_ai.targetEnemy != null)
        {
            Debug.Log(_ai.gameObject.name + ": " + _ai.targetEnemy.name + " を追跡開始");
        }
    }

    public void OnUpdate()
    {
        //ターゲットを見失っら索敵状態に戻る
        if (_ai.targetEnemy == null)
        {
            _ai.ChangeState(new AI_MoveToTargetAreaState(_ai));
            return;
        }

        //ターゲットとの距離を計算
        float distanceToEnemy = Vector3.Distance(_ai.transform.position, _ai.targetEnemy.position);

        //攻撃範囲内にいるか
        if (distanceToEnemy <= _ai.attackRange)
        {
            _ai.ChangeState(new AI_AttackingState(_ai)); 
        }
        //索敵範囲外に出てしまったか
        else if (distanceToEnemy > _ai.sightRange)
        {
            _ai.targetEnemy = null;
            _ai.ChangeState(new AI_MoveToTargetAreaState(_ai));
        }
        //攻撃範囲外だが、索敵範囲内か
        else
        {
            _ai.Movement.SetTarget(_ai.targetEnemy.position);
        }
    }

    public void OnExit()
    {
        //念のため動きを止める
        _ai.Movement.Stop();
    }
}
