using UnityEngine;
using Assets.Code.Classes.Bullets;

namespace Assets.Code.Classes.Enemies
{
    [RequireComponent (typeof (Enemy))]
    class RailTurret : MonoBehaviour
    {
        [Tooltip ("The amount of damage each bullet does.")]
        [SerializeField] private int _Damage = 2;
        [Tooltip ("How long each bullet will stay on screen for.")]
        [SerializeField] private float _LifeTime = 5;
        [Tooltip ("The speed of each bullet.")]
        [SerializeField] private float _BulletSpeed = 10f;
        [Tooltip ("The rate at which the turret will shoot bullets.")]
        [SerializeField] private float _FireRate = 0.0f;
        [SerializeField] private float _ShootDelay = 0.1f;
        [Tooltip ("The bullets that the turret will fire.")]
        [SerializeField] private GameObject _BulletPrefab = null;
        [Tooltip ("The anchor for where bullet's spawn from.")]
        [SerializeField] private Transform _WeaponAnchor = null;

        private float _Timer = 0.0f;
        private bool _ShouldTrack = true;
        private Transform _Target = null;
        private Transform _Transform = null;

        private void Awake ()
        {
            _Transform = GetComponent<Transform> ();
        }

        private void Start ()
        {
            var targets = GameObject.FindObjectsOfType<PlayerController> ();
            _Target = targets[Random.Range (0, targets.Length)].transform;
        }

        private void Update ()
        {
            CalculateTimer ();

            if (CanFire ())
            {
                Shoot ();
            }

            if (_ShouldTrack)
                TrackPlayer ();
        }

        private void CalculateTimer ()
        {
            _Timer += Time.deltaTime;
        }

        private void TrackPlayer ()
        {
            var offset = _Transform.position - _Target.position;
            _Transform.LookAt (_Transform.position + offset);
            //_Transform.rotation = Quaternion.LookRotation (-_Target.position + _Transform.position);
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
    }
}
