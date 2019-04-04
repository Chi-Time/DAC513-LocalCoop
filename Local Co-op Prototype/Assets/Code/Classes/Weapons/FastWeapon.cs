using UnityEngine;
using Assets.Code.Classes.Bullets;

namespace Assets.Code.Classes.Weapons
{
    class FastWeapon : WeaponBase
    {
        protected override void Shoot ()
        {
            var bulletObj = Instantiate (_BulletPrefab, _WeaponAnchor.transform.position, _WeaponAnchor.transform.rotation);

            var bullet = bulletObj.GetComponent<Bullet> ();
            bullet.Constructor (_Damage, _BulletSpeed, _LifeTime);

            bullet.transform.SetParent (_BulletHolder);
        }
    }
}
