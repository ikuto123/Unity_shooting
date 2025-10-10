using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private IPlayerAction _currentAction;
    private PlayerMoveInput _playerMoveInput;
    private PlayerWeaponInput _playerWeaponInput = new PlayerWeaponInput();
    
    CharacterManager _characterManager;
    private void Start()
    {
        _characterManager = GetComponent<CharacterManager>();
        _playerMoveInput = new PlayerMoveInput(GetComponent<Rigidbody>());
    }
    
    //プレイヤーからの入力を受け付けた処理
    private void Update()
    {
        _playerWeaponInput.GunHolder(_characterManager);
        
        _playerMoveInput.ReadInput();
    }
    
    private void FixedUpdate()
    {
        _playerMoveInput.Movement();
    }
    
}
