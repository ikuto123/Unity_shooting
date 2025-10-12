using AIState;
using UnityEngine;

public class AICombat
{
    private readonly AIController _controller;
    private readonly CharacterManager _characterManager; // ★キャラクターマネージャーへの参照

    public AICombat(AIController controller)
    {
        _controller = controller;
        _characterManager = controller.CharacterManager; 
    }

    public void Attack()
    {
        // ターゲットが設定されていなければ何もしない
        if (_controller.targetEnemy == null) return;
        
        // 攻撃の瞬間に、正確に敵の方向を向く
        _controller.transform.LookAt(_controller.targetEnemy);

       if (_characterManager.WeaponManager != null)
        {
            _characterManager.WeaponManager.FireCurrentGun(_characterManager);
        }
    }
}
