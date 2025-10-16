using UnityEngine;

public class PlayerMoveInput
{
    private float _moveSpeed = 3f;
    private float _strafeSpeedMultiplier = 0.5f;
    
    private Rigidbody _rb;
    private Vector3 _moveDirection;
    
    public PlayerMoveInput(Rigidbody rb)
    {
        this._rb = rb;
    }

    public Vector3 MoveDirection => _moveDirection;
    
    public void ReadInput()
    {
        // 入力方向をリセット
        _moveDirection = Vector3.zero;
        
        if (Input.GetKey(KeyCode.W))
        {
            _moveDirection += Vector3.forward; 
            
        }
        if (Input.GetKey(KeyCode.S))
        {
            _moveDirection += Vector3.back;    
        }
        if (Input.GetKey(KeyCode.D))
        {
            _moveDirection += Vector3.right;  
        }
        if (Input.GetKey(KeyCode.A))
        {
            _moveDirection += Vector3.left;  
        }
        
        if (_moveDirection.magnitude > 1)
        {
            _moveDirection = _moveDirection.normalized;
        }
    }
    
    public void Movement()
    {
// 適用する基本速度を決定
        float currentSpeed = _moveSpeed;
        
        if (_moveDirection.z <= 0)
        {
            currentSpeed *= _strafeSpeedMultiplier;
        }
        
        Vector3 worldMoveDirection = _rb.transform.TransformDirection(_moveDirection);
        
        Vector3 targetVelocity = new Vector3(worldMoveDirection.x * currentSpeed, _rb.linearVelocity.y, worldMoveDirection.z * currentSpeed);
        _rb.linearVelocity = targetVelocity;
    }
    
    public void StopMovement()
    {
        _rb.linearVelocity = Vector3.zero;
    }
}

