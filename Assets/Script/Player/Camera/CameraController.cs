using UnityEngine;

public class CameraController
{
    private GameObject camera;
    private CameraInput cameraInput;
    private float sensitivity;
    public CameraController(GameObject camera ,float sensitivity)
    {
        this.camera = camera;
        cameraInput = new CameraInput();
        this.sensitivity = sensitivity;
    }

    public void CameraRotation()
    {
        Vector3 input = cameraInput.MouseInputDistance() * sensitivity;

        // 現在の回転を取得
        Vector3 currentEuler = camera.transform.rotation.eulerAngles;

        // 新しい回転を適用（Z軸は固定）
        float newX = currentEuler.x - input.y;
        float newY = currentEuler.y + input.x;

        camera.transform.rotation = Quaternion.Euler(newX, newY, 0f);
    }

}
