using UnityEngine;

public class MouseInput
{
    private float _inputX;
    private float _inputY;

    public Vector3 MouseInputDistance()
    {
        _inputX = Input.GetAxis("Mouse X");
        _inputY = Input.GetAxis("Mouse Y");
        return new Vector3(_inputX, _inputY, 0);
    }
}
