using AIState;
using UnityEngine;

public class AI_AttackingState : IState
{
    private AIController _ai;
    
    public AI_AttackingState(AIController ai)
    {
        _ai = ai;
    }
    
    public void OnEnter()
    {
        _ai.Movement.Stop();
        Debug.Log("攻撃状態に入りました！");
    }

    public void OnUpdate()
    {
        //ターゲットを見失った場合、移動状態に遷移
        if (_ai.targetEnemy == null)
        {
            _ai.ChangeState(new AI_MoveToTargetAreaState(_ai));
            return;
        }
        float distanceToEnemy = Vector3.Distance(_ai.transform.position, _ai.targetEnemy.position);
        
        if (distanceToEnemy > _ai.attackRange)
        {
            _ai.ChangeState(new AI_ChaseEnemyState(_ai));
            return; 
        }
        Vector3 directionToLook = _ai.targetEnemy.position - _ai.transform.position;
        directionToLook.y = 0;
        if (directionToLook != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToLook);
            _ai.transform.rotation = Quaternion.Slerp(_ai.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        
        _ai.Combat.Attack();
    }

    public void OnExit()
    {
        Debug.Log("攻撃状態を終了します");
    }
}
