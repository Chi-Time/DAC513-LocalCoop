using UnityEngine;

namespace Assets.Code.Classes.Bullets
{
    [RequireComponent (typeof (Rigidbody), typeof (Collider))]
    class SpongeBullet : MonoBehaviour
    {
        private int _Damage = 0;
        private float _Speed = 0.0f;
        private float _LifeTime = 0.0f;
        [SerializeField] private float _XMin = 0.0f;
        [SerializeField] private float _XMax = 0.0f;
        private Rigidbody _Rigidbody = null;
        private Transform _Transform = null;

        public void Constructor (int damage, float speed)
        {
            _Damage = damage;
            _Speed = speed;
            _LifeTime = float.MaxValue;
            Setup ();
        }

        public void Constructor (int damage, float speed, float lifeTime)
        {
            _Damage = damage;
            _Speed = speed;
            _LifeTime = lifeTime;
            Setup ();
        }

        public void Setup ()
        {
            Invoke ("Cull", _LifeTime);
            _Rigidbody.AddForce (_Transform.up * _Speed, ForceMode.Impulse);

            var bounds = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0));
            _XMin = -bounds.x + transform.localScale.x;
            _XMax = bounds.x - transform.localScale.x;
        }

        private void Awake ()
        {
            AssignReferences ();
        }

        private void AssignReferences ()
        {
            GetComponent<Collider> ().isTrigger = true;
            _Transform = GetComponent<Transform> ();
            _Rigidbody = GetComponent<Rigidbody> ();
            _Rigidbody.useGravity = false;
            _Rigidbody.isKinematic = false;
            _Rigidbody.freezeRotation = true;
        }

        private void FixedUpdate ()
        {
            if (_Rigidbody.position.x <= _XMin)
            {
                _Transform.rotation = Quaternion.Euler (_Transform.rotation.eulerAngles.x, -_Transform.rotation.eulerAngles.y, _Transform.rotation.eulerAngles.z);

                _Rigidbody.velocity = Vector3.zero;
                _Rigidbody.AddForce (_Transform.up * _Speed, ForceMode.Impulse);
            }

            if (_Rigidbody.position.x >= _XMax)
            {
                _Transform.rotation = Quaternion.Euler (_Transform.rotation.eulerAngles.x, -_Transform.rotation.eulerAngles.y, _Transform.rotation.eulerAngles.z);

                _Rigidbody.velocity = Vector3.zero;
                _Rigidbody.AddForce (_Transform.up * _Speed, ForceMode.Impulse);
            }
        }

        private void Cull ()
        {
            Destroy (this.gameObject);
        }

        private void OnBecameInvisible ()
        {
            Cull ();
        }
    }
}
