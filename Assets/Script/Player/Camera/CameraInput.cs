using UnityEngine;

public class CameraInput
{
    private float InputX;
    private float InputY;

    public Vector3 MouseInputDistance()
    {
        InputX = Input.GetAxis("Mouse X");
        InputY = Input.GetAxis("Mouse Y");
        return new Vector3(InputX, InputY, 0);
    }
}
