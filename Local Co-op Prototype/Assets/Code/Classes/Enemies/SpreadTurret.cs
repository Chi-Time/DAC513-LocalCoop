using UnityEngine;
using Assets.Code.Classes.Bullets;

namespace Assets.Code.Classes.Enemies
{
    class SpreadTurret : MonoBehaviour
    {
        [Tooltip ("The amount of damage each bullet does.")]
        [SerializeField] private int _Damage = 2;
        [Tooltip ("How long each bullet will stay on screen for.")]
        [SerializeField] private float _LifeTime = 5;
        [Tooltip ("The speed of each bullet.")]
        [SerializeField] private float _BulletSpeed = 10f;
        [SerializeField] private float _ShotFireRate = 2.0f;
        [Tooltip ("The rate at which the turret will shoot bullets.")]
        [SerializeField] private float _FireRate = 0.25f;
        [Tooltip ("The length of time the turret should fire for before stopping.")]
        [SerializeField] private float _FireLength = 0.0f;
        [Tooltip ("The bullets that the turret will fire.")]
        [SerializeField] private GameObject _BulletPrefab = null;
        [Tooltip ("The anchor for where bullet's spawn from.")]
        [SerializeField] private Transform _WeaponAnchor = null;
        
        private float _Timer = 0.0f;
        private bool _IsActivated = false;
        private float _ShotLengthTimer = 0.0f;
        private bool _IsFiring = false;
        private Transform _Target = null;
        private Transform _Transform = null;

        private void Awake ()
        {
            _Transform = GetComponent<Transform> ();
        }

        private void Start ()
        {
            var targets = FindObjectsOfType<PlayerController> ();
            _Target = targets[Random.Range (0, targets.Length)].transform;
        }

        private void Update ()
        {
            if (_IsActivated)
            {
                CalculateTimer ();

                if (CanFire () && _IsFiring)
                {
                    Shoot ();
                }

                TrackPlayer ();
            }
        }

        private void CalculateTimer ()
        {
            if (_IsFiring == false)
            {
                _ShotLengthTimer += Time.deltaTime;

                if (_ShotLengthTimer > _ShotFireRate)
                {
                    _IsFiring = true;
                    _ShotLengthTimer = 0.0f;
                }
            }
            else
            {
                _ShotLengthTimer += Time.deltaTime;

                if (_ShotLengthTimer >= _FireLength)
                {
                    _IsFiring = false;
                    _ShotLengthTimer = 0.0f;
                }
            }

            _Timer += Time.deltaTime;
        }

        private void TrackPlayer ()
        {
            var offset = _Transform.position - _Target.position;
            _Transform.LookAt (_Transform.position + offset);
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
            var bulletObj = Instantiate (_BulletPrefab, _WeaponAnchor.transform.position, _WeaponAnchor.transform.rotation);

            bulletObj.transform.rotation = Quaternion.Euler (90f, _WeaponAnchor.transform.rotation.eulerAngles.y, _WeaponAnchor.transform.rotation.eulerAngles.z); 

            Debug.Log (bulletObj.transform.rotation);

            var bullet = bulletObj.GetComponent<Bullet> ();
            bullet.Constructor (_Damage, _BulletSpeed, _LifeTime);
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
