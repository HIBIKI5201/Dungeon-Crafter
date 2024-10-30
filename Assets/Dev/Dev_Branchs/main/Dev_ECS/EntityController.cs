using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;

namespace DCFrameWork
{
    [RequireComponent (typeof(NavMeshAgent), typeof(Rigidbody))]
    public class EntityController : MonoBehaviour
    {
        [SerializeField]
        Vector3 target = new Vector3(10, 0, -10);
        NavMeshAgent _agent;
        [SerializeField]
        float _moveSpeed;
        Rigidbody _rigidbody;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody> ();
            _agent = GetComponent<NavMeshAgent>();
            _agent.updatePosition = false;
            _agent.SetDestination(target);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                ManualMove();
            }
        }

        private void ManualMove()
        {
            Vector3 direction = (_agent.steeringTarget - transform.position).normalized; // êiÇﬁï˚å¸ÇåvéZ
            _rigidbody.linearVelocity = direction * _moveSpeed; // à⁄ìÆ
            Debug.Log($"move\ndirection is {direction}\nvelocity is {_rigidbody.linearVelocity}");
        }
    }
}
