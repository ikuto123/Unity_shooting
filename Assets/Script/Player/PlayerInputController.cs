using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private IPlayerAction _currentAction;
    private PlayerMoveInput _playerMoveInput;
    private PlayerWeaponInput _playerWeaponInput = new PlayerWeaponInput();
    private CameraController _cameraController;
    
    [Header("Camera")]
    private Transform _player;
    private float _sensitivity = 0.5f;//カメラ感度
    
    CharacterManager _characterManager;
    private void Start()
    {
        _player = this.gameObject.transform;
        _cameraController = new CameraController(_player , _sensitivity);
        _characterManager = GetComponent<CharacterManager>();
        _playerMoveInput = new PlayerMoveInput(GetComponent<Rigidbody>());
    }
    
    //プレイヤーからの入力を受け付けた処理
    private void Update()
    {
        _playerWeaponInput.GunHolder(_characterManager);
        
        _playerMoveInput.ReadInput();
        
        _cameraController.CameraRotation();
    }
    
    private void FixedUpdate()
    {
        _playerMoveInput.Movement();
    }
    
}
