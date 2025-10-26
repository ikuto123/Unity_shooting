public interface IWeapon
{
    int GunID { get; }
    string GunName { get; }
    int EnergyCost { get; }
    
    float BeamSpeed { get; }    
    float BeamLifetime { get; } 
    bool Fire(IChargeable user);
}