using UnityEngine;
using Assets.Code.Classes.Bullets;

namespace Assets.Code.Classes.Weapons
{
    class SpeedySpreadShot : WeaponBase
    {
        [Tooltip ("The min and max angle the bullet can be fired at.")]
        [SerializeField] private float _SpreadAngle = 15f;

        protected override void Shoot ()
        {
            var bulletObj = Instantiate (_BulletPrefab, _WeaponAnchor.transform.position, _WeaponAnchor.transform.rotation);

            bulletObj.transform.rotation = Quaternion.Euler (new Vector3 (
                    _WeaponAnchor.transform.rotation.eulerAngles.x,
                    _WeaponAnchor.transform.rotation.eulerAngles.x + Random.Range (-_SpreadAngle, _SpreadAngle),
                    _WeaponAnchor.transform.rotation.eulerAngles.x
                ));

            var bullet = bulletObj.GetComponent<Bullet> ();
            bullet.Constructor (_Damage, _BulletSpeed, _LifeTime);

            

            bullet.transform.SetParent (_BulletHolder);
        }
    }
}
