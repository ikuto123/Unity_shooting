using AIState;
using UnityEngine;

public class AI_SearchItemState : IState
{
    private AIController _ai;
    
    public AI_SearchItemState(AIController ai)
    {
        _ai = ai;
    }
    
    public void OnEnter()
    {
        Debug.Log("攻撃状態に入りました！");
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        Debug.Log("攻撃状態を終了します");
    }
}
