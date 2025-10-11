using UnityEngine;

namespace Beam
{
    public class BeamMove : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private float _speed;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Initialize(float speed)
        {
            _speed = speed;
        }

        private void OnEnable()
        {
            _rigidbody.linearVelocity = transform.forward * _speed;
        }

        private void OnDisable()
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }
}