using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private IPlayerAction _currentAction;
    private PlayerMoveInput _playerMoveInput;
    private PlayerWeaponInput _playerWeaponInput = new PlayerWeaponInput();
    private CameraController _cameraController;
    
    [Header("Camera")]
    [SerializeField] private GameObject camera;
    [SerializeField] private float sensitivity;//カメラ感度
    
    CharacterManager _characterManager;
    private void Start()
    {
        _cameraController = new CameraController(camera , sensitivity);
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
