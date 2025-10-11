using UnityEngine;

namespace AIState
{
    public class AIController : MonoBehaviour
    {
        private IState _currentState;
        public AICharacterSensor CharacterSensor { get; private set; }
        public AIMovement Movement { get; private set; }
        public CharactorTeam MyTeam { get; private set; }
        
        [Header("移動パラメータ")]
        public float moveSpeed = 3.5f;
        
        [Header("索敵パラメータ")]
        public float sightRange = 15f; // 索敵範囲（距離）
        [Range(0, 360)] public float sightAngle = 90f; // 索敵範囲（角度）
        public LayerMask obstacleLayer; // 障害物（壁など）のレイヤー
        
        [Header("ターゲット情報")]
        public Transform targetEnemy { get; set; }
        public Vector3 targetAreaPosition { get; set; }

        private void Awake()
        {
            MyTeam = GetComponent<CharactorTeam>();
            CharacterSensor = new AICharacterSensor(this);
            Movement = new AIMovement(this);
            
        }

        private void Start()
        {
            ChangeState(new AI_IdleState(this));
            if (GameManager.Instance != null)
            {
                targetAreaPosition = GameManager.Instance.GetTargetAreaPosition();
            }
        }

        private void Update()
        {
            _currentState?.OnUpdate();
        }
        
        private void FixedUpdate()
        {
            Movement.Movement();
        }

        public void ChangeState(IState nextState)
        {
            _currentState?.OnExit();
            _currentState = nextState;
            _currentState.OnEnter();
        }
        
        //デバッグ用
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            // --- 索敵範囲（距離）をワイヤーの球で表示 ---
            Gizmos.color = Color.yellow; // 色を黄色に設定
            Gizmos.DrawWireSphere(transform.position, sightRange);

            // --- 索敵範囲（視野角）を扇形で表示 ---
            UnityEditor.Handles.color = new Color(0, 0, 1, 0.7f); // 色を半透明の黄色に設定
        
            // 視野角の開始地点のベクトルを計算
            Vector3 from = Quaternion.Euler(0, -sightAngle / 2, 0) * transform.forward;
        
            // 扇形を描画
            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                Vector3.up,        
                from,              
                sightAngle,        
                sightRange    
            );
        }
#endif
    }
}