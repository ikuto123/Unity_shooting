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
        _ai.targetEnemy = null; 
        
        _ai.Movement.SetTarget(_ai.targetAreaPosition);
    }

    public void OnUpdate()
    {
        var foundEnemy = _ai.CharacterSensor.FindEnemy();
        //敵を発見した場合
        if (foundEnemy != null)
        {
            _ai.targetEnemy = foundEnemy;
            //追跡状態に移行する
            _ai.ChangeState(new AI_ChaseEnemyState(_ai)); 
        }
    }

    public void OnExit()
    {
    }
}
