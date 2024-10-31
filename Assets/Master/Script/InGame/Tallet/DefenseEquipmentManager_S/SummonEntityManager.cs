using DCFrameWork.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace DCFrameWork
{
    [RequireComponent (typeof(NavMeshAgent))]
    public class SummonEntityManager : MonoBehaviour
    {

        void Start()
        {
        
        }

        void Update()
        {
        
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IFightable component))
            {
                
            }
        }
    }
}
