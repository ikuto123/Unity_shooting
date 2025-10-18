using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AIState
{
    public class AIController : MonoBehaviour
    {
        private IState _currentState;
        public AICharacterSensor CharacterSensor { get; private set; }
        public AIMovement Movement { get; private set; }
        public CharactorTeam MyTeam { get; private set; }
        public AICombat Combat { get; private set; }
        public CharactorAnimator CharacterAnimator { get; private set; }
      
        private CharacterManager _characterManager;
        
        [Header("AI武器切り替え設定")]
        private float _weaponSwitchCheckInterval = 7.0f;
        private float _weaponChengeMinInterval = 5.0f;
        private float _weaponChengeManInterval = 10.0f;
        
        private float _weaponSwitchTimer = 0f;
        
        public float moveSpeed { get; private set; } 
        
        [Header("索敵パラメータ")]
        public float sightRange; // 索敵範囲（距離）
        [Range(0, 360)] public float sightAngle ; // 索敵範囲（視野角）
        public LayerMask obstacleLayer; // 障害物（壁など）のレイヤー
        
        [Header("攻撃パラメータ")]
        public float attackRange; // 攻撃可能距離
        
        [Header("ターゲット情報")]
        public Transform targetEnemy { get; set; }
        public Vector3 targetAreaPosition { get; set; }
        public CharacterManager CharacterManager { get; private set; }
        
        private void Awake()
        {
            CharacterManager = GetComponent<CharacterManager>();
            MyTeam = GetComponent<CharactorTeam>();
            _characterManager = GetComponent<CharacterManager>();
            CharacterAnimator = GetComponentInChildren<CharactorAnimator>();
            CharacterSensor = new AICharacterSensor(this);
            Combat = new AICombat(this);
            Movement = new AIMovement(this);
            
        }

        private void Start()
        {
            moveSpeed = _characterManager.MoveSpeed;
            ChangeState(new AI_IdleState(this));
            
            if (GameManager.Instance != null)
            {
                targetAreaPosition = GameManager.Instance.OccupationManager.GetTargetAreaPosition();
            }
        }

        private void Update()
        {
            _currentState?.OnUpdate();
            
            HandleWeaponSwitching();
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

        private void HandleWeaponSwitching()
        {
            _weaponSwitchTimer += Time.deltaTime;

            if (_weaponSwitchTimer >= _weaponSwitchCheckInterval)
            {
                _weaponSwitchTimer = 0f; 
                _weaponSwitchCheckInterval = Random.Range(_weaponChengeMinInterval, _weaponChengeManInterval);
                List<int> availableIDs = CharacterManager.WeaponManager.GetWeaponIDs();
                
                int currentWeaponID = CharacterManager.WeaponManager.GunID;
                List<int> otherWeaponIDs = availableIDs.Where(id => id != currentWeaponID).ToList();

                if (otherWeaponIDs.Count > 0)
                {
                    
                    int randomIndex = Random.Range(0, otherWeaponIDs.Count);
                    int newWeaponID = otherWeaponIDs[randomIndex];
                    
                    CharacterManager.WeaponManager.SelectGun(newWeaponID);
                    Debug.Log($"{gameObject.name}が武器をID {newWeaponID} にランダムで切り替えました。");
                }
            }
        }
        
        //デバッグ用
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            // --- 索敵範囲（距離）を黄色のワイヤー球で表示 ---
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawWireSphere(transform.position, sightRange);
            
            // --- 攻撃範囲（距離）を赤色のワイヤー球で表示 ---
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            // --- 索敵範囲（視野角）を半透明の黄色い扇形で表示 ---
            UnityEditor.Handles.color = new Color(1, 1, 0, 0.7f); 
            Vector3 from = Quaternion.Euler(0, -sightAngle / 2, 0) * transform.forward;
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