using UnityEngine;

public class ChargeWeapon : WeaponBaseClass
{
    private bool _isCharging = false;
    private float _chargeStartTime = 0f;
    private readonly float _requiredChargeTime = 0.7f;// チャージに必要な時間（秒）
    
    private float _chargeDuration = 0f;
    public ChargeWeapon(int id, string name, int energyCost, int damage,
        float beamSpeed, float beamLifetime, int maxActiveBeam, float fireRate ) :
        base(id, name, energyCost, damage, beamSpeed, beamLifetime, maxActiveBeam, fireRate) { }

    public override bool Fire(IChargeable user)
    {
        // チャージ中であれば、チャージ時間を計算
        if (!_isCharging)
        {
            // 【新要件】発射クールダウン中であれば、チャージを開始しない
            if (Time.time < _nextFireTime)
            {
                return false; 
            }
            
            // エネルギーが足りなければチャージを開始できない
            if (user == null || !user.HasEnoughCharge(EnergyCost))
            {
                return false;
            }
            
            // 全ての条件をクリアしたらチャージ開始
            _isCharging = true;
            _chargeStartTime = Time.time;
            Debug.Log("チャージ開始！");
            return false; // このフレームでは発射しない
        }
        
        _chargeDuration = Time.time - _chargeStartTime;

        // 3. チャージ時間が規定に満たなければ、まだ発射しない
        if (_chargeDuration < _requiredChargeTime)
        {
            return false;
        }
        
        Debug.Log("チャージ完了！発射します。");
        
        bool canFire = base.Fire(user);
        
        ResetCharge();

        if (canFire)
        {
            // 実際に弾を生成・発射する処理をここに追加
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
