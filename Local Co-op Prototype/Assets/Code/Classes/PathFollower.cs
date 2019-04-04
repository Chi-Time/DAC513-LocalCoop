using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PathFollower : MonoBehaviour
{
    [SerializeField] private float _Speed = 3f;
    [SerializeField] private Transform _PathParent = null;

    private int _Index;
    private Transform _Transform = null;
    private Rigidbody _Rigidbody = null;
    private Transform _TargetPoint = null;

    private void Awake ()
    {
        AssignReferences ();
        SetupRigidbody ();
        RemoveEditorPathing ();
    }

    private void AssignReferences ()
    {
        _Transform = GetComponent<Transform> ();
        _Rigidbody = GetComponent<Rigidbody> ();
    }

    private void SetupRigidbody ()
    {
        _Rigidbody.isKinematic = true;
        _Rigidbody.useGravity = false;
        _Rigidbody.freezeRotation = true;
    }

    private void RemoveEditorPathing ()
    {
        for (int i = 0; i < _PathParent.childCount; i++)
        {
            var child = _PathParent.GetChild (i);
            Destroy (child.GetComponent<MeshRenderer> ());
            Destroy (child.GetComponent<Collider> ());
            Destroy (child.GetComponent<MeshFilter> ());
        }
    }

    private void Start ()
    {
        SetDefaults ();
    }

    private void SetDefaults ()
    {
        _Index = 0;
        _TargetPoint = _PathParent.GetChild (_Index);
        _Transform.up = _TargetPoint.position - _Transform.position;
    }

    private void Update ()
    {
        MoveToPathPoint ();
        CheckPosition ();
    }

    private void MoveToPathPoint ()
    {
        _Rigidbody.MovePosition (Vector3.MoveTowards (_Rigidbody.position, _TargetPoint.position, _Speed * Time.fixedDeltaTime));
    }

    private void CheckPosition ()
    {
        if (HasReachedNextPathPoint ())
        {
            // Increment to the next path point and find our new position.
            _Index++;
            _Index %= _PathParent.childCount;
            _TargetPoint = _PathParent.GetChild (_Index);
            _Transform.forward = _TargetPoint.position - _Transform.position;
        }
    }

    private bool HasReachedNextPathPoint ()
    {
        if (Vector3.Distance (_Transform.position, _TargetPoint.position) < 0.1f)
            return true;

        return false;
    }

    private void OnDrawGizmos ()
    {
        Vector3 from = Vector3.zero;
        Vector3 to = Vector3.zero;

        for (int i = 0; i < _PathParent.childCount; i++)
        {
            from = _PathParent.GetChild (i).position;
            to = _PathParent.GetChild ((i + 1) % _PathParent.childCount).position;

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine (from, to);
        }
    }
}
