using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraModeChanger
{
    CinemachineCamera _mainPlayerCamera;
    CinemachineCamera _fPSCamera;
    
    public event Action OnCameraModeFPS;
    public event Action OnCameraModeTPS;
    
    CharactorAnimator _playerAnimator;
    public CameraModeChanger(CinemachineCamera MainPlayerCamera , CinemachineCamera FPSCamera , CharactorAnimator playerAnimator)
    {
        _mainPlayerCamera = MainPlayerCamera;
        _fPSCamera = FPSCamera;
        _playerAnimator = playerAnimator;
    }
    
    //FPSとTPSの切り替え
    public void ChangeCameraMode()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _mainPlayerCamera.gameObject.SetActive(false);
            _fPSCamera.gameObject.SetActive(true);
            _playerAnimator.Fire(true);
            OnCameraModeFPS?.Invoke();
        }
        if(Input.GetMouseButtonUp(1)) { 
            _mainPlayerCamera.gameObject.SetActive(true);
            _fPSCamera.gameObject.SetActive(false);
            _playerAnimator.Fire(false);
            OnCameraModeTPS?.Invoke();
        }
    }
}
