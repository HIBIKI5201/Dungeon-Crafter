using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class CameraManager : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField]
    private float _linearSpeed = 1;
    [SerializeField]
    private float _angularSpeed = 1;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
    }

    public void CameraMove(Vector2 axis, byte rotate)
    {
        _rigidbody.linearVelocity = new Vector3(axis.x, 0, axis.y) * _linearSpeed;
        _rigidbody.angularVelocity = Vector3.up * rotate * _angularSpeed;
    }
}
