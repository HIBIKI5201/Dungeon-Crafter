using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;

namespace DCFrameWork
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
    public class EntityController : MonoBehaviour
    {
        [SerializeField]
        Vector3 _target = new Vector3(10, 0, -10);
        NavMeshAgent _agent;
        [SerializeField]
        float _moveSpeed;
        Rigidbody _rigidbody;

        [SerializeField]
        bool _updatePos;
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _agent = GetComponent<NavMeshAgent>();
            _agent.updatePosition = _updatePos;
            _agent.SetDestination(_target);
        }

        private void Update()
        {
            ManualMove();
        }

        private void ManualMove()
        {
            //�o�H���v�Z����Ă��邩���m�F
            if (_agent.pathPending) return;
            // �ʒu�̍X�V
            transform.position = _agent.nextPosition;
        }
    }
}
