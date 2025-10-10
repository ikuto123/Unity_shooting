using UnityEngine;

public class PlayerMoveInput
{
    private float _moveSpeed = 5f;
    
    private Rigidbody _rb;
    private Vector3 _moveDirection;
    
    public PlayerMoveInput(Rigidbody rb)
    {
        this._rb = rb;
    }

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
        Vector3 targetVelocity = new Vector3(_moveDirection.x * _moveSpeed, _rb.linearVelocity.y, _moveDirection.z * _moveSpeed);

        _rb.linearVelocity = targetVelocity;
    }
}

