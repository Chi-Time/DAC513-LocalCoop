using UnityEngine;
using Assets.Code.Classes.Bullets;

namespace Assets.Code.Classes.Weapons
{
    class TriShot : WeaponBase
    {
        [Tooltip ("The angular distance between each bullet.")]
        [SerializeField] private float _Angle = 35f;

        protected override void Shoot ()
        {
            // We loop three times so that we get 3 bullets. But we start from -1 so that.
            // We can multiply per angle using i.
            int start = -1;
            int end = 2;

            for (int i = start; i < end; i++)
            {
                var bulletObj = Instantiate (
                    _BulletPrefab, 
                    _WeaponAnchor.transform.position, 
                    _WeaponAnchor.transform.rotation
                );

                bulletObj.transform.rotation = Quaternion.Euler (new Vector3 (
                    _WeaponAnchor.transform.rotation.eulerAngles.x,
                    _WeaponAnchor.transform.rotation.eulerAngles.x + (i * _Angle), 
                    _WeaponAnchor.transform.rotation.eulerAngles.x
                ));

                var bullet = bulletObj.GetComponent<Bullet> ();
                bullet.Constructor (_Damage, _BulletSpeed, _LifeTime);

                bullet.transform.SetParent (_BulletHolder);
            }
        }
    }
}
