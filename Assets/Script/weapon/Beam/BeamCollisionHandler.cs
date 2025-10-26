using UnityEngine;

namespace Beam
{
    public class BeamCollisionHandler : MonoBehaviour
    {
        private BeamManager _beamManager;
        private void Awake()
        {
            _beamManager = GetComponent<BeamManager>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("衝突" + collision.gameObject.name);
            _beamManager.OnCollision(collision);
        }
    }
}