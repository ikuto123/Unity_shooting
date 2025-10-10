using UnityEngine;

public class NomalWeapon : WeaponBaseClass
{
    public NomalWeapon(int id, string name, int energyCost, int damage , float beamSpeed, float beamLifetime)
        : base(id, name, energyCost,damage , beamSpeed, beamLifetime) { }

    public override bool Fire(IChargeable user)
    {
        // まず親クラスのFireを呼び、チャージ消費が成功したかを確認
        bool firedSuccessfully = base.Fire(user);

        // 成功した場合のみ、この武器固有の処理を行う
        if (firedSuccessfully)
        {
            Debug.Log(GunName + "を発射しました！");
        }
        
        return firedSuccessfully;
    }
}
