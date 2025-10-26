using UnityEngine;

public class CharactorAnimator : MonoBehaviour
{
    private Animator _animator;
    
    //ハッシュ値を設定しておく
    private static readonly int _isPlayerSquatHash = Animator.StringToHash("isPlayerSquat");
    private static readonly int _isWalkHash = Animator.StringToHash("IsWalk");
    private static readonly int _isWalkRightHash = Animator.StringToHash("IsWalkRight");
    private static readonly int _isWalkLeftHash = Animator.StringToHash("IsWalkLeft");
    private static readonly int _isFireHash = Animator.StringToHash("IsFire");
    private static  readonly int _isWalkBackHash = Animator.StringToHash("IsWalkBack");
    
    private static readonly int _isRunHash = Animator.StringToHash("IsRun");
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Squat(bool isSquatting)
    {
        _animator.SetBool(_isPlayerSquatHash, isSquatting);
    }

    public void Fire(bool isFiring)
    {
        _animator.SetBool(_isFireHash, isFiring);
    }
    
    public void SetRun(bool isRunning)
    {
        _animator.SetBool(_isRunHash, isRunning);
    }
    
    public void UpdateMovementAnimation(Vector3 localMoveDirection)
    {
        //前後方向の移動
        bool isWalkingForward = localMoveDirection.z > 0.1f;
        _animator.SetBool(_isWalkHash, isWalkingForward);
        
        //後ろ向け
        bool isWalkingBack = localMoveDirection.z < -0.1f;
        _animator.SetBool(_isWalkBackHash, isWalkingBack);

        //左右方向の移動 
        bool isWalkingRight = localMoveDirection.x > 0.1f;
        bool isWalkingLeft = localMoveDirection.x < -0.1f;
        _animator.SetBool(_isWalkRightHash, isWalkingRight);
        _animator.SetBool(_isWalkLeftHash, isWalkingLeft);

    }

}
