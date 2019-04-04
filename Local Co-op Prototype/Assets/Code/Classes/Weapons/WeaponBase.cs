using UnityEngine;

namespace Assets.Code.Classes.Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [Tooltip ("The amount of damage the bullet does upon impacting an enemy.")]
        [SerializeField] protected int _Damage = 0;
        [Tooltip ("The speed at which the bullet moves across the screen.")]
        [SerializeField] protected float _BulletSpeed = 0.0f;
        [Tooltip ("How long the bullet lives for before being culled.")]
        [SerializeField] protected float _LifeTime = 0.0f;
        [Tooltip ("The speed at which this weapon fires bullets.")]
        [SerializeField] protected float _FireRate = 0.25f;
        [Tooltip ("The bullet object spawn when firing this weapon.")]
        [SerializeField] protected GameObject _BulletPrefab = null;
        [Tooltip ("The point at which bullets come out from.")]
        [SerializeField] protected Transform _WeaponAnchor = null;

        protected float _Timer = 0.0f;
        protected Transform _BulletHolder = null;

        //TODO: Make it so that player character generates all of the default weapons on game start.
        // So make it like an array of prefabs that the player character spawns 
        // thereby ensuring it set's itself up correctly.
        protected virtual void Awake ()
        {
            foreach (Transform child in this.transform)
                _WeaponAnchor = child;

            SetBulletHolder ();
        }

        protected void SetBulletHolder ()
        {
            var bulletHolder = GameObject.Find ("Bullet Holder");

            if (bulletHolder == null)
                _BulletHolder = new GameObject ("Bullet Holder").transform;
            else
                _BulletHolder = bulletHolder.transform;
        }

        protected virtual void Update ()
        {
            CalculateTimer ();
        }

        protected void CalculateTimer ()
        {
            _Timer += Time.deltaTime;
        }

        public void Fire ()
        {
            if (CanFire ())
            {
                Shoot ();
            }
        }

        protected bool CanFire ()
        {
            if (_Timer >= _FireRate)
            {
                _Timer = 0.0f;
                return true;
            }

            return false;
        }

        protected abstract void Shoot ();

        public virtual void Enable ()
        {
            this.enabled = true;
        }

        public virtual void Disable ()
        {
            this.enabled = false;
        }
    }
}
