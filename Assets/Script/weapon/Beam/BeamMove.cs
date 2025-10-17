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
            if(this.gameObject.activeSelf) UpdateVelocity();
        }

        private void OnEnable()
        {
            //UpdateVelocity();
        }

        // 進行方向を更新するためのメソッド
        public void UpdateVelocity()
        {
            if (_rigidbody != null)
            {
                _rigidbody.linearVelocity = transform.forward * _speed;
            }
        }

        private void OnDisable()
        {
            if(_rigidbody != null)
            {
                _rigidbody.linearVelocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
            }
        }
    }
}