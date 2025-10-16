using UnityEngine;

public class CharactorAnimator : MonoBehaviour
{
    private Animator _animator;
    
    private static readonly int IsPlayerSquatHash = Animator.StringToHash("isPlayerSquat");
    private static readonly int IsWalkHash = Animator.StringToHash("IsWalk");
    private static readonly int IsWalkRightHash = Animator.StringToHash("IsWalkRight");
    private static readonly int IsWalkLeftHash = Animator.StringToHash("IsWalkLeft");
    private static readonly int IsFireHash = Animator.StringToHash("IsFire");
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Squat(bool isSquatting)
    {
        _animator.SetBool(IsPlayerSquatHash, isSquatting);
    }

    public void Fire(bool isFiring)
    {
        _animator.SetBool(IsFireHash, isFiring);
    }
    
    public void UpdateMovementAnimation(Vector3 localMoveDirection)
    {
        // 前後方向の移動 (Z軸)
        bool isWalkingForward = localMoveDirection.z > 0.1f;
        _animator.SetBool(IsWalkHash, isWalkingForward);
        
        //後ろ向け(予定)
        /*bool isWalkingBack = localMoveDirection.z < -0.1f;
        _animator.SetBool(IsWalkHash, isWalkingBack);*/

        // 左右方向の移動 (X軸)
        bool isWalkingRight = localMoveDirection.x > 0.1f;
        bool isWalkingLeft = localMoveDirection.x < -0.1f;
        _animator.SetBool(IsWalkRightHash, isWalkingRight);
        _animator.SetBool(IsWalkLeftHash, isWalkingLeft);

    }

}
