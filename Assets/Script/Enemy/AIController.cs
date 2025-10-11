using UnityEngine;

namespace AIState
{
    public class AIController
    {
        private IState _currentState;

        void Start()
        {
            // 初期ステートを設定
            ChangeState(new IdleState(this));
        }

        void Update()
        {
            _currentState?.OnUpdate();
        }

        //ステートの変更の際に呼び出される
        public void ChangeState(IState nextState)
        {
            _currentState?.OnExit(); 
            _currentState = nextState;
            _currentState.OnEnter(); 
        }
    }
}
