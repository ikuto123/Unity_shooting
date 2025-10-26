using AIState;
using UnityEngine;

public class AI_IdleState : IState
{
    private readonly AIController _ai;

    public AI_IdleState(AIController ai)
    {
        _ai = ai;
    }

    public void OnEnter()
    {
        
    }

    public void OnUpdate()
    {
        //目標エリアへの移動状態へ移行する
        _ai.ChangeState(new AI_MoveToTargetAreaState(_ai));
    }

    public void OnExit()
    {
        
    }
}
