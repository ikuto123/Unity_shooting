using AIState;
using UnityEngine;

public class AI_MoveToTargetAreaState : IState
{
    private AIController _ai;
    
    public AI_MoveToTargetAreaState(AIController ai)
    {
        _ai = ai;
    }
    public void OnEnter()
    {
        Debug.Log("占領エリアに向かいます");
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        Debug.Log("攻撃状態を終了します");
    }
}
