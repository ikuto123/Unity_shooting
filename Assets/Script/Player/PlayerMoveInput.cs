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
        // _moveDirectionをプレイヤーのローカル座標系からワールド座標系に変換する
        Vector3 worldMoveDirection = _rb.transform.TransformDirection(_moveDirection);

        // 変換されたワールド座標の方向を使って速度を計算する
        // Y軸（垂直方向）の速度は現在のものを維持する
        Vector3 targetVelocity = new Vector3(worldMoveDirection.x * _moveSpeed, _rb.linearVelocity.y, worldMoveDirection.z * _moveSpeed);

        _rb.linearVelocity = targetVelocity;
    }
}

