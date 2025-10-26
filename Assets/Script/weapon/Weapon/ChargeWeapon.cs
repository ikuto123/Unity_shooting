using UnityEngine;

public class ChargeWeapon : WeaponBaseClass
{
    private bool _isCharging = false;
    private float _chargeStartTime = 0f;
    private readonly float _requiredChargeTime = 0.7f;//チャージに必要な時間
    
    private float _chargeDuration = 0f;
    public ChargeWeapon(int id, string name, int energyCost, int damage,
        float beamSpeed, float beamLifetime, int maxActiveBeam, float fireRate ) :
        base(id, name, energyCost, damage, beamSpeed, beamLifetime, maxActiveBeam, fireRate) { }

    public override bool Fire(IChargeable user)
    {
        //チャージ時間を計算
        if (!_isCharging)
        {
            //発射クールダウン中であれば、チャージを開始しない
            if (Time.time < _nextFireTime)
            {
                return false; 
            }
            
            //エネルギーが足りなければチャージを開始できない
            if (user == null || !user.HasEnoughCharge(EnergyCost))
            {
                return false;
            }
            
            //チャージ開始
            _isCharging = true;
            _chargeStartTime = Time.time;
            Debug.Log("チャージ開始！");
            return false; 
        }
        
        _chargeDuration = Time.time - _chargeStartTime;

        //チャージ時間が規定に満たなければ発射しない
        if (_chargeDuration < _requiredChargeTime)
        {
            return false;
        }
        
        Debug.Log("チャージ完了！発射します。");
        
        bool canFire = base.Fire(user);
        
        ResetCharge();

        if (canFire)
        {
            Debug.Log($"{GunName} を発射！");
        }

        return canFire;
        
    }
    public void CancelCharge()
    {
        if (_isCharging)
        {
            Debug.Log("チャージが中断されました。");
            ResetCharge();
        }
    }

    private void ResetCharge()
    {
        _isCharging = false;
        _chargeDuration = 0f;
        _chargeStartTime = 0f;
    }
}
