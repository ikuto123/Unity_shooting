using UnityEngine;

public class CameraController
{
    private Transform _player;
    private MouseInput _playerInput;
    private float _sensitivity;

    private float _pitch = 0f; // X軸用のみ保持！

    public CameraController(Transform player, float sensitivity)
    {
        _player = player;
        _playerInput = new MouseInput();
        _sensitivity = sensitivity;
    }

    public void CameraRotation()
    {
        Vector3 input = _playerInput.MouseInputDistance() * _sensitivity;

        // X軸（上下）だけを計算して、自前で保持
        _pitch -= input.y;

        // X軸のみ制限
        _pitch = Mathf.Clamp(_pitch, -20f, 20f);

        // Y軸（左右）は制限なし
        float yaw = _player.rotation.eulerAngles.y + input.x;

        // X軸（上下）= _pitch（制限済）
        // Y軸（左右）= yaw
        // Z軸 = 0で固定
        _player.rotation = Quaternion.Euler(_pitch, yaw, 0f);
    }
}