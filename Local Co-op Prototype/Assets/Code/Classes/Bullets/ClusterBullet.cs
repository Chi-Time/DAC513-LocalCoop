using UnityEngine;

namespace Assets.Code.Classes.Bullets
{
    [RequireComponent (typeof (Rigidbody), typeof (Collider))]
    class ClusterBullet : MonoBehaviour
    {
        private int _Damage = 0;
        private float _Speed = 0.0f;
        private float _LifeTime = 0.0f;
        private Rigidbody _Rigidbody = null;
        private Transform _Transform = null;
        private Bullet[] _SubBullets = null;

        public void Constructor (int damage, float speed, Bullet[] bullets)
        {
            _Damage = damage;
            _Speed = speed;
            _LifeTime = float.MaxValue;
            _SubBullets = bullets;
            Setup ();
        }

        public void Constructor (int damage, float speed, float lifeTime, Bullet[] bullets)
        {
            _Damage = damage;
            _Speed = speed;
            _LifeTime = lifeTime;
            _SubBullets = bullets;
            Setup ();
        }

        public void Setup ()
        {
            CancelInvoke ();
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

        private void Split ()
        {
            foreach (Bullet bullet in _SubBullets)
            {
                bullet.transform.position = _Transform.position;
                bullet.gameObject.SetActive (true);
                bullet.Setup ();
            }
        }

        private void Cull ()
        {
            Split ();
            Destroy (this.gameObject);
        }
    }
}
