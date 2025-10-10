using UnityEngine;

public interface IChargeable
{
    int CurrentCharge { get; }
    int MaxCharge { get; }
    bool HasEnoughCharge(int cost);
    void UseCharge(int cost);
}
