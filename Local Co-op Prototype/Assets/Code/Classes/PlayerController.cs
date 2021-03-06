﻿using UnityEngine;
using Assets.Code.Enums;
using Assets.Code.Classes.Weapons;
using Assets.Code.Classes.Utilities;

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

        _CurrentWeapon = GetComponentInChildren<DefaultWeapon> ();
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
        if (Input.GetKeyDown (KeyCode.E))
            SwitchWeapon (WeaponTypes.ClusterShot);

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

    /// <summary>Switch the players current weapon to that of the given type. Disabling any currently equipped weapon in the process.</summary>
    /// <param name="weapon">The weapon to switch to.</param>
    public void SwitchWeapon (WeaponTypes weapon)
    {
        switch (weapon)
        {
            case WeaponTypes.Default:
                _CurrentWeapon.Disable ();
                _CurrentWeapon = GetComponent<DefaultWeapon> ();
                _CurrentWeapon.Enable ();
                break;
            case WeaponTypes.Fast:
                _CurrentWeapon.Disable ();
                _CurrentWeapon = GetComponentInChildren<FastWeapon> ();
                _CurrentWeapon.Enable ();
                break;
            case WeaponTypes.TriShot:
                _CurrentWeapon.Disable ();
                _CurrentWeapon = GetComponentInChildren<TriShot> ();
                _CurrentWeapon.Enable ();
                break;
            case WeaponTypes.PentaShot:
                _CurrentWeapon.Disable ();
                _CurrentWeapon = GetComponentInChildren<PentaShot> ();
                _CurrentWeapon.Enable ();
                break;
            case WeaponTypes.HomingShot:
                _CurrentWeapon.Disable ();
                _CurrentWeapon = GetComponentInChildren<DefaultWeapon> ();
                _CurrentWeapon.Enable ();
                break;
            case WeaponTypes.ClusterShot:
                _CurrentWeapon.Disable ();
                _CurrentWeapon = GetComponentInChildren<ClusterShotWeapon> ();
                _CurrentWeapon.Enable ();
                break;
            case WeaponTypes.SpeedySpreadShot:
                _CurrentWeapon.Disable ();
                _CurrentWeapon = GetComponentInChildren<SpeedySpreadShot> ();
                _CurrentWeapon.Enable ();
                break;
            case WeaponTypes.SpongeShot:
                _CurrentWeapon.Disable ();
                _CurrentWeapon = GetComponentInChildren<SpongeShotWeapon> ();
                _CurrentWeapon.Enable ();
                break;
        }
    }

    private void OnEnable ()
    {
        LevelSignals.OnEntityHit += OnEntityHit;
    }

    private void OnEntityHit (int damage, GameObject entity)
    {
        if (entity == this.gameObject)
        {
            _Health -= damage;
            if (_Health <= 0)
                Kill ();
        }
    }

    private void Kill ()
    {
        Destroy (this.gameObject);
    }

    private void OnDisable ()
    {
        LevelSignals.OnEntityHit -= OnEntityHit;
    }
}
