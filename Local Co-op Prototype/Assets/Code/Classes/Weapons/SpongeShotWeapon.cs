using UnityEngine;
using Assets.Code.Classes.Bullets;

namespace Assets.Code.Classes.Weapons
{
    class SpongeShotWeapon : WeaponBase
    {
        [Tooltip ("The angle at which to fire the bullet.")]
        [SerializeField] private float _Angle = 45.0f;

        protected override void Shoot ()
        {
            _Angle = Random.Range (0, 1) == 0 ? -_Angle : _Angle;

            var bulletObj = Instantiate (_BulletPrefab, _WeaponAnchor.transform.position, _WeaponAnchor.transform.rotation);

            bulletObj.transform.rotation = Quaternion.Euler (new Vector3 (
                    _WeaponAnchor.transform.rotation.eulerAngles.x,
                    _WeaponAnchor.transform.rotation.eulerAngles.x + _Angle,
                    _WeaponAnchor.transform.rotation.eulerAngles.x
                ));

            var bullet = bulletObj.GetComponent<SpongeBullet> ();
            bullet.Constructor (_Damage, _BulletSpeed, _LifeTime);

            bullet.transform.SetParent (_BulletHolder);
        }
    }
}
