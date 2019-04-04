using UnityEngine;
using Assets.Code.Classes.Bullets;

namespace Assets.Code.Classes.Weapons
{
    class ClusterShotWeapon : WeaponBase
    {
        [Tooltip ("The amount of damage the bullet does upon impacting an enemy.")]
        [SerializeField] protected int _ClusterDamage = 0;
        [Tooltip ("The speed at which the bullet moves across the screen.")]
        [SerializeField] protected float _ClusterBulletSpeed = 0.0f;
        [Tooltip ("How long the bullet lives for before being culled.")]
        [SerializeField] protected float _ClusterLifeTime = 0.0f;
        [SerializeField] private ClusterBullet _ClusterPrefab = null;
        [Tooltip ("The amount of bullets to spawn upon the main bullet dying.")]
        [SerializeField] private int _ClusterAmount = 0;

        protected override void Shoot ()
        {
            var subBullets = new Bullet[_ClusterAmount];

            float angleStep = 360 / _ClusterAmount;
            float currentAngle = 0.0f;

            for (int i = 0; i < subBullets.Length; i++)
            {
                var bulletObj = Instantiate (
                    _BulletPrefab,
                    _WeaponAnchor.transform.position,
                    _WeaponAnchor.transform.rotation
                    );

                bulletObj.transform.rotation = Quaternion.Euler (new Vector3 (
                    _WeaponAnchor.transform.rotation.eulerAngles.x,
                    currentAngle,
                    _WeaponAnchor.transform.rotation.eulerAngles.z
                ));

                currentAngle += angleStep;

                var bullet = bulletObj.GetComponent<Bullet> ();
                bullet.Constructor (_Damage, _BulletSpeed, _LifeTime);

                bullet.transform.SetParent (_BulletHolder);
                bullet.gameObject.SetActive (false);
                subBullets[i] = bullet;
            }

            var clusterObj = Instantiate (_ClusterPrefab, _WeaponAnchor.transform.position, _WeaponAnchor.transform.rotation);

            var cluster = clusterObj.GetComponent<ClusterBullet> ();
            cluster.Constructor (_ClusterDamage, _ClusterBulletSpeed, _ClusterLifeTime, subBullets);

            cluster.transform.SetParent (_BulletHolder);
        }
    }
}
