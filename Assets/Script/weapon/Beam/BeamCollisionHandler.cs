using UnityEngine;

namespace Beam
{
    public class BeamCollisionHandler : MonoBehaviour
    {
        // 衝突イベントを定義。衝突相手と、この弾の情報(Beam)を渡す
        private BeamManager _beamManager;
        private void Awake()
        {
            _beamManager = GetComponent<BeamManager>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("衝突" + collision.gameObject.name);
            // 衝突したことを通知するだけ
            _beamManager.ExecuteHitEffect(collision);
        }
    }
}