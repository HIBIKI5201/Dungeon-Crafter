using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField]
    private float _speed = 1;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void CameraMove(Vector2 axis)
    {
        _rigidbody.linearVelocity = new Vector3(axis.x, 0, axis.y) * _speed;
    }
}
