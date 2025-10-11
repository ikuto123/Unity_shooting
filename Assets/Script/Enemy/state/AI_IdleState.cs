using AIState;
using UnityEngine;

public class AI_IdleState : IState
{
    private AIController _owner;
    public AI_IdleState(AIController ai)
    {
        _owner = ai;
    }
    
    public void OnEnter()
    {
        Debug.Log("待機状態に入りました");
    }

    public void OnUpdate()
    {
        // スペースキーが押されたら、攻撃状態に切り替える
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _owner.ChangeState(new AI_AttackingState(_owner));
        }
    }

    public void OnExit()
    {
        Debug.Log("待機状態を終了します");
    }
}
