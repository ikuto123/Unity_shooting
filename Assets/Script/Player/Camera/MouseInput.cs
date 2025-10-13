using UnityEngine;

public class MouseInput
{
    private float _InputX;
    private float _InputY;

    public Vector3 MouseInputDistance()
    {
        _InputX = Input.GetAxis("Mouse X");
        _InputY = Input.GetAxis("Mouse Y");
        return new Vector3(_InputX, _InputY, 0);
    }
}
