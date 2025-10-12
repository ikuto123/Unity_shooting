using UnityEngine;

public class RecoveryArea : MonoBehaviour
{
    private int HPamount = 1;
    private int Chargeamount = 1;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<IRecover>(out var target))
        {
            Debug.Log("Recovery");
            target.RecoverHp(HPamount);
            target.RecoverCharge(Chargeamount);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {

    }
    
}
