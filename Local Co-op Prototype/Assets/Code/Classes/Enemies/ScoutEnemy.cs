using UnityEngine;
using Assets.Code.Classes.Utilities;

namespace Assets.Code.Classes.Enemies
{
    [RequireComponent (typeof (Rigidbody), typeof (Collider))]
    class ScoutEnemy : MonoBehaviour
    {
        [SerializeField] private float _Speed = 10.0f;

        private Transform _Target = null;
        private Rigidbody _RigidBody = null;

        private void Awake ()
        {
            GetComponent<Collider> ().isTrigger = true;
        }

        private void Start ()
        {
            var targets = GameObject.FindObjectsOfType<PlayerController> ();

            _Target = targets[Random.Range (0, targets.Length)].transform;
        }

        private void OnTriggerEnter (Collider other)
        {
            if (other.name == "Initialise")
            {
                iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath ("Movement"), "axis", "y", "orienttopath", true, "speed", _Speed, "easetype", iTween.EaseType.easeInOutSine, "islocal", false));
            }

            if (other.name == "Deactivate")
            {
                iTween.Stop (this.gameObject);
            }
        }
    }
}
