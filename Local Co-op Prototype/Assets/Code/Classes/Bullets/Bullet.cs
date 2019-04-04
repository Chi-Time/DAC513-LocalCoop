using UnityEngine;

namespace Assets.Code.Classes.Bullets
{
    [RequireComponent (typeof (Rigidbody), typeof (Collider))]
    public class Bullet : MonoBehaviour
    {
        private int _Damage = 0;
        private float _Speed = 0.0f;
        private float _LifeTime = 0.0f;
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

        private void Cull ()
        {
            Destroy (this.gameObject);
        }

        private void OnBecameInvisible ()
        {
            Cull ();
        }

        private void OnTriggerEnter (Collider other)
        {
            Cull ();
            Utilities.LevelSignals.HitEntity (_Damage, other.gameObject);
        }
    }
}
