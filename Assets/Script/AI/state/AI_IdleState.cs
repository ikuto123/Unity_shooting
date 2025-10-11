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
        Debug.Log("待機状態に入りました");
    }

    public void OnUpdate()
    {
        // 即座に目標エリアへの移動状態へ移行する
        _ai.ChangeState(new AI_MoveToTargetAreaState(_ai));
    }

    public void OnExit()
    {
        Debug.Log("待機状態を終了します");
    }
}
