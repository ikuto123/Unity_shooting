using Unity.Cinemachine;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private CharactorAnimator _playerAnimator;
    
    private IPlayerAction _currentAction;
    private PlayerMoveInput _playerMoveInput;
    private PlayerWeaponInput _playerWeaponInput = new PlayerWeaponInput();
    private CameraController _cameraController;
    private CameraModeChanger _cameraModeChanger;
    
    [Header("Camera")]
    [SerializeField] private CinemachineCamera _MainPlayerCamera;
    [SerializeField] private CinemachineCamera _FPSCamera;
    
    private Transform _player;
    private float _sensitivity = 2f;//カメラ感度
    
    CharacterManager _characterManager;
    private void Start()
    {
        _player = this.gameObject.transform;
        _cameraController = new CameraController(_player , _sensitivity);
        _cameraModeChanger = new CameraModeChanger(_MainPlayerCamera , _FPSCamera , _playerAnimator);
        _characterManager = GetComponent<CharacterManager>();
        _playerMoveInput = new PlayerMoveInput(GetComponent<Rigidbody>());
    }
    
    //プレイヤーからの入力を受け付けた処理
    private void Update()
    {
        _playerWeaponInput.GunHolder(_characterManager);
        _cameraController.CameraRotation();
        
        if (Input.GetKey(KeyCode.LeftShift))
        { 
            _playerAnimator.Squat(true);
            _playerMoveInput.StopMovement();
            return;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) { _playerAnimator.Squat(false); }
        
        _playerMoveInput.ReadInput();
        _playerAnimator.UpdateMovementAnimation(_playerMoveInput.MoveDirection);
        _cameraModeChanger.ChangeCameraMode();
    }
    
    private void FixedUpdate()
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            _playerMoveInput.Movement();
        }
    }
    
}
