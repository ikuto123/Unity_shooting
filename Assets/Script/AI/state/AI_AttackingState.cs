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
        _ai.CharacterAnimator?.Fire(true);
    }

    public void OnUpdate()
    {
        //ターゲットを見失っら移動状態に
        if (_ai.targetEnemy == null)
        {
            _ai.ChangeState(new AI_MoveToTargetAreaState(_ai));
            return;
        }
        float distanceToEnemy = Vector3.Distance(_ai.transform.position, _ai.targetEnemy.position);
        
        //アタックの距離より離れたら敵を追いかけるステートに移行
        if (distanceToEnemy > _ai.attackRange)
        {
            _ai.ChangeState(new AI_ChaseEnemyState(_ai));
            return; 
        }
        
        var currentWeapon = _ai.CharacterManager.WeaponManager.CurrentGun;
        if (currentWeapon == null) return;
        //チャージが残っているかどうか
        if (_ai.CharacterManager.CurrentCharge < currentWeapon.EnergyCost)
        {
            
            //チャージが切れた場合回復エリアを探す
            _ai.ChangeState(new AI_ChargeWeaponState(_ai));
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
        _ai.CharacterAnimator?.Fire(false);
    }
}
