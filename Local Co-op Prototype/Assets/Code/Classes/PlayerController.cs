using UnityEngine;
using Assets.Code.Enums;

[RequireComponent (typeof (Rigidbody), typeof (SphereCollider))]
public class PlayerController : MonoBehaviour
{
    [Tooltip ("The amount of health this player has.")]
    [SerializeField] private int _Health = 100;
    [Tooltip ("How fast this player is able to move across the screen.")]
    [SerializeField] private float _Speed = 15.0f;
    [Tooltip ("What player input does this player use?")]
    [SerializeField] private PlayerTypes _PlayerType = PlayerTypes.Player1;

    private Vector3 _Velocity = Vector3.zero;
    private Rigidbody _Rigidbody = null;

    private void Awake ()
    {
        AssignReferences ();
    }

    private void AssignReferences ()
    {
        GetComponent<SphereCollider> ().isTrigger = false;

        _Rigidbody = GetComponent<Rigidbody> ();
        _Rigidbody.useGravity = false;
        _Rigidbody.isKinematic = true;
        _Rigidbody.freezeRotation = true;
        _Rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
    }

    private void Update ()
    {
        GetInput ();
    }

    private void GetInput ()
    {
        switch (_PlayerType)
        {
            case PlayerTypes.Player1:
                GetMovementInput (1);
                break;
            case PlayerTypes.Player2:
                GetMovementInput (2);
                break;
        }
    }

    private void GetMovementInput (int playerID)
    {
        string horizontal = "Horizontal" + playerID;
        string vertical = "Vertical" + playerID;
        _Velocity = new Vector3 (Input.GetAxis (horizontal), 0.0f, Input.GetAxis (vertical));
    }

    private void FixedUpdate ()
    {
        Move ();
    }

    private void Move ()
    {
        var velocity = _Velocity * _Speed * Time.fixedDeltaTime;
        _Rigidbody.MovePosition (_Rigidbody.position + velocity);
    }
}
