using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private CharacterManager characterManager;
    void Start()
    {
        
        characterManager = GetComponent<CharacterManager>();
        characterManager.WeaponManager.FireCurrentGun(characterManager);
    }

    void Update()
    {
        Debug.Log("自分の体力");
        
    }

}
