using UnityEngine;
using Assets.Code.Classes.Bullets;

namespace Assets.Code.Classes.Enemies
{
    [RequireComponent (typeof (Enemy), typeof (Rigidbody), typeof (Collider))]
    class SteamRollerEnemy : MonoBehaviour
    {
        [Tooltip ("The speed at which the enemy moves down the screen.")]
        [SerializeField] private float _Speed = 5.0f;
        [Tooltip ("The amount of damage each bullet does.")]
        [SerializeField] private int _Damage = 2;
        [Tooltip ("How long each bullet will stay on screen for.")]
        [SerializeField] private float _LifeTime = 5;
        [Tooltip ("The speed of each bullet.")]
        [SerializeField] private float _BulletSpeed = 10f;
        [Tooltip ("How many bullets should the turret fire?")]
        [SerializeField] private int _ShotAmount = 5;
        [Tooltip ("The rate at which the turret will shoot bullets.")]
        [SerializeField] private float _FireRate = 0.0f;
        [Tooltip ("The bullets that the turret will fire.")]
        [SerializeField] private GameObject _BulletPrefab = null;

        private float _Timer = 0.0f;
        private bool _IsActivated = true;
        private Rigidbody _Rigidbody = null;
        private Transform _Transform = null;

        private void Awake ()
        {
            GetComponent<Collider> ().isTrigger = true;

            _Rigidbody = GetComponent<Rigidbody> ();
            _Rigidbody.useGravity = false;
            _Rigidbody.isKinematic = true;
            _Rigidbody.freezeRotation = true;
            _Transform = GetComponent<Transform> ();
        }

        private void Update ()
        {
            if (_IsActivated)
            {
                CalculateTimer ();

                if (CanFire ())
                    Shoot ();
            }
        }

        private void CalculateTimer ()
        {
            _Timer += Time.deltaTime;
        }

        private bool CanFire ()
        {
            if (_Timer >= _FireRate)
            {
                _Timer = 0.0f;
                return true;
            }

            return false;
        }

        private void Shoot ()
        {
            float angleStep = 360 / _ShotAmount;
            float currentAngle = 0.0f;

            for (int i = 0; i < _ShotAmount; i++)
            {
                var bulletObj = Instantiate (_BulletPrefab, _Transform.position, _Transform.rotation);

                currentAngle += angleStep;
                bulletObj.transform.rotation = Quaternion.Euler (90f, currentAngle, 0f);

                var bullet = bulletObj.GetComponent<Bullet> ();
                bullet.Constructor (_Damage, _BulletSpeed, _LifeTime);
            }
        }

        private void OnTriggerEnter (Collider other)
        {
            if (other.name == "Initialise")
            {
                _IsActivated = true;
            }

            if (other.name == "Deactivate")
            {
                _IsActivated = false;
            }
        }
    }
}
