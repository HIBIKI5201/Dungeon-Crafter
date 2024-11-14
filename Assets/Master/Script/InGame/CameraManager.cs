using UnityEngine;

namespace DCFrameWork.SceneSystem
{
    [RequireComponent(typeof(Rigidbody))]
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

        public void CameraMove(Vector2 direction, float rotate)
        {
            _rigidbody.linearVelocity = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up) * new Vector3(direction.x, 0, direction.y) * _linearSpeed;
            _rigidbody.angularVelocity = Vector3.up * rotate * _angularSpeed;
        }
    }
}