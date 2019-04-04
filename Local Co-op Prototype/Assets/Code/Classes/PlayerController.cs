using UnityEngine;
using Assets.Code.Enums;
using Assets.Code.Classes.Weapons;

[RequireComponent (typeof (Rigidbody), typeof (SphereCollider))]
public class PlayerController : MonoBehaviour
{
    [Tooltip ("The amount of health this player has.")]
    [SerializeField] private int _Health = 100;
    [Tooltip ("How fast this player is able to move across the screen.")]
    [SerializeField] private float _Speed = 15.0f;
    [Tooltip ("What player input does this player use?")]
    [SerializeField] private PlayerTypes _PlayerType = PlayerTypes.Player1;

    private float _XMin = 0.0f;
    private float _XMax = 0.0f;
    private float _ZMin = 0.0f;
    private float _ZMax = 0.0f;
    private WeaponBase _CurrentWeapon = null;
    private Vector3 _Velocity = Vector3.zero;
    private Rigidbody _Rigidbody = null;

    private void Awake ()
    {
        AssignReferences ();
        GetScreenBounds ();
    }

    private void AssignReferences ()
    {
        GetComponent<SphereCollider> ().isTrigger = false;

        _Rigidbody = GetComponent<Rigidbody> ();
        _Rigidbody.useGravity = false;
        _Rigidbody.isKinematic = true;
        _Rigidbody.freezeRotation = true;
        _Rigidbody.constraints = RigidbodyConstraints.FreezePositionY;

        _CurrentWeapon = GetComponentInChildren<SpeedySpreadShot> ();
    }

    private void GetScreenBounds ()
    {
        var bounds = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0));

        _XMin = -bounds.x + transform.localScale.x;
        _XMax = bounds.x - transform.localScale.x;
        _ZMin = -bounds.z + transform.localScale.y;
        _ZMax = bounds.z - transform.localScale.y;
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
                GetFireInput (1);
                break;
            case PlayerTypes.Player2:
                GetMovementInput (2);
                GetFireInput (2);
                break;
        }
    }

    private void GetMovementInput (int playerID)
    {
        string horizontal = "Horizontal" + playerID;
        string vertical = "Vertical" + playerID;
        _Velocity = new Vector3 (Input.GetAxis (horizontal), 0.0f, Input.GetAxis (vertical));
    }

    private void GetFireInput (int playerID)
    {
        string fire = "Fire" + playerID;

        if (Input.GetButton (fire) && _CurrentWeapon != null)
        {
            _CurrentWeapon.Fire ();
        }
    }

    private void FixedUpdate ()
    {
        ClampWithinScreen ();
        Move ();
    }

    private void Move ()
    {
        var velocity = _Velocity * _Speed * Time.fixedDeltaTime;
        _Rigidbody.MovePosition (_Rigidbody.position + velocity);
    }

    private void ClampWithinScreen ()
    {
        if (_Rigidbody.position.x <= _XMin)
        {
            _Rigidbody.position = new Vector3 (_XMin, 0.0f, _Rigidbody.position.z);
        }

        if (_Rigidbody.position.x >= _XMax)
        {
            _Rigidbody.position = new Vector3 (_XMax, 0.0f, _Rigidbody.position.z);
        }

        if (_Rigidbody.position.z <= _ZMin)
        {
            _Rigidbody.position = new Vector3 (_Rigidbody.position.x, 0.0f, _ZMin);
        }

        if (_Rigidbody.position.z >= _ZMax)
        {
            _Rigidbody.position = new Vector3 (_Rigidbody.position.x, 0.0f, _ZMax);
        }
    }
}
