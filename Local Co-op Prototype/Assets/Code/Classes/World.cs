using UnityEngine;

namespace Assets.Code.Classes
{
    class World : MonoBehaviour
    {
        [Tooltip ("The speed at which all objects move across the world.")]
        [SerializeField] private float _Speed = 0.0f;

        private Transform _Transform = null;

        private void Awake ()
        {
            _Transform = GetComponent<Transform> ();
        }

        private void FixedUpdate ()
        {
            _Transform.Translate (Vector3.back * _Speed * Time.deltaTime, Space.World);
        }
    }
}
