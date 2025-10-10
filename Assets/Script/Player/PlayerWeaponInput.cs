using UnityEngine;

public class PlayerWeaponInput
{
    public void GunHolder(CharacterManager character)
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { character.WeaponManager.SelectGun(1); }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { character.WeaponManager.SelectGun(2); }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) { character.WeaponManager.SelectGun(3); }
        
        if (Input.GetMouseButtonDown(0)){ character.WeaponManager.FireCurrentGun(character); }
    }
}
