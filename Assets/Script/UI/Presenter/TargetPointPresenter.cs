using UnityEngine;

public class TargetPointPresenter
{
    public TargetPointPresenter(CameraModeChanger cameraModeChanger , GameObject target_TPS , GameObject target_FPS)
    {
        target_TPS.SetActive(true);
        target_FPS.SetActive(false);
        
        cameraModeChanger.OnCameraModeFPS += () =>
        {
            target_TPS.SetActive(false);
            target_FPS.SetActive(true);
        };
        
        cameraModeChanger.OnCameraModeTPS += () =>
        {
            target_TPS.SetActive(true);
            target_FPS.SetActive(false);
        };
    }
    
}
