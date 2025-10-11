using AIState;
using UnityEngine;

public class AI_MoveToTargetAreaState : IState
{
    private readonly AIController _ai;

    public AI_MoveToTargetAreaState(AIController ai)
    {
        _ai = ai;
    }

    public void OnEnter()
    {
        Debug.Log("目標エリアに向かいます");
        _ai.targetEnemy = null; 
        
        _ai.Movement.SetTarget(_ai.targetAreaPosition);
    }

    public void OnUpdate()
    {
        var foundEnemy = _ai.CharacterSensor.FindEnemy();

        // 敵を発見した場合
        if (foundEnemy != null)
        {
            Debug.Log(_ai.gameObject.name + "が敵を発見！ -> " + foundEnemy.name);
            // ターゲット情報を更新
            _ai.targetEnemy = foundEnemy;
            // 追跡状態に移行する
            Debug.Log("敵を発見しました");
            //_ai.ChangeState(new AI_ChaseEnemyState(_ai)); // ※追跡用のStateが別途必要
        }
    }

    public void OnExit()
    {
        Debug.Log("目標エリアへの移動を終了します");
    }
}
