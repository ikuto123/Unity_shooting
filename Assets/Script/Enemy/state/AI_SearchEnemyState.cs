using AIState;
using UnityEngine;

public class AI_SearchEnemyState : IState
{
    private AIController _ai;
    
    public AI_SearchEnemyState(AIController ai)
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
