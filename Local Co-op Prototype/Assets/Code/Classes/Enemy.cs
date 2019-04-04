using UnityEngine;
using Assets.Code.Classes.Utilities;

namespace Assets.Code.Classes
{
    class Enemy : MonoBehaviour
    {
        [Tooltip ("The amount of damage the enemy does upon colliding.")]
        [SerializeField] private int _Damage = 2;
        [Tooltip ("How much health does this enemy have?")]
        [SerializeField] private int _Health = 100;
        [Tooltip ("Ho many points does this enemy give off after being killed?")]
        [SerializeField] private int _Value = 10;
        [Tooltip ("Can this enemy be flown into by the player?")]
        [SerializeField] private bool _CanBeCollided = false;
        [Tooltip ("Should the enemy die upon being collided with?")]
        [SerializeField] private bool _ShouldDieUponCollision = true;

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
            if (_CanBeCollided && other.CompareTag ("Player"))
            {
                if (_ShouldDieUponCollision)
                {
                    Kill ();
                }

                LevelSignals.HitEntity (_Damage, other.gameObject);
            }
        }
    }
}
