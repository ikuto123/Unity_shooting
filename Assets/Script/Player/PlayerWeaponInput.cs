using UnityEngine;

public class PlayerWeaponInput
{
    //銃に関する入力処理
    public void GunHolder(CharacterManager character)
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { character.WeaponManager.SelectGun(1); }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { character.WeaponManager.SelectGun(2); }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) { character.WeaponManager.SelectGun(3); }
        
        if (Input.GetMouseButton(0)) { character.WeaponManager?.FireCurrentGun(character); }
        
        if (Input.GetMouseButtonUp(0)) {if (Input.GetMouseButtonUp(0))
        {
            if (character.WeaponManager.CurrentGun is ChargeWeapon chargeWeapon)
            {
                chargeWeapon.CancelCharge();
            }
        } }
    }
}
