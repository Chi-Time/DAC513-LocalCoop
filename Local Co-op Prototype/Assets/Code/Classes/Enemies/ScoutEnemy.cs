using UnityEngine;
using Assets.Code.Classes.Utilities;

namespace Assets.Code.Classes.Enemies
{
    [RequireComponent (typeof (Rigidbody), typeof (Collider))]
    class ScoutEnemy : MonoBehaviour
    {
        [SerializeField] private int _Health = 100;
        [SerializeField] private int _Value = 10;
        [SerializeField] private int _Damage = 2;

        private void Awake ()
        {
            GetComponent<Collider> ().isTrigger = true;
        }

        private void OnEnable ()
        {
            LevelSignals.OnEntityHit += OnEntityHit;
        }

        private void OnEntityHit (int damage, GameObject entity)
        {
            if (entity == this.gameObject)
            {
                _Health -= damage;
                if (_Health <= 0)
                    Kill ();
            }
        }

        private void Kill ()
        {
            Destroy (this.gameObject);
            LevelSignals.IncreaseScore (_Value);
        }

        private void OnDisable ()
        {
            LevelSignals.OnEntityHit -= OnEntityHit;
        }

        private void OnTriggerEnter (Collider other)
        {
            if (other.CompareTag ("Player"))
            {
                LevelSignals.HitEntity (_Damage, other.gameObject);
                Kill ();
            }
        }
    }
}
