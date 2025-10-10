public interface IWeapon
{
    int GunID { get; }
    string GunName { get; }
    int EnergyCost { get; }
    
    float BeamSpeed { get; }    // 追加
    float BeamLifetime { get; } // 追加
    bool Fire(IChargeable user);
}