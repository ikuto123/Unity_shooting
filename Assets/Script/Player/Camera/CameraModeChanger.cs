using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraModeChanger
{
    CinemachineCamera _MainPlayerCamera;
    CinemachineCamera _FPSCamera;
    
    public event Action OnCameraModeFPS;
    public event Action OnCameraModeTPS;
    
    CharactorAnimator _playerAnimator;
    public CameraModeChanger(CinemachineCamera MainPlayerCamera , CinemachineCamera FPSCamera , CharactorAnimator playerAnimator)
    {
        _MainPlayerCamera = MainPlayerCamera;
        _FPSCamera = FPSCamera;
        _playerAnimator = playerAnimator;
    }
    
    public void ChangeCameraMode()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("押されています");
            _MainPlayerCamera.gameObject.SetActive(false);
            _FPSCamera.gameObject.SetActive(true);
            _playerAnimator.Fire(true);
            OnCameraModeFPS?.Invoke();
        }
        if(Input.GetMouseButtonUp(1)) { 
            _MainPlayerCamera.gameObject.SetActive(true);
            _FPSCamera.gameObject.SetActive(false);
            _playerAnimator.Fire(false);
            OnCameraModeTPS?.Invoke();
        }
    }
}
