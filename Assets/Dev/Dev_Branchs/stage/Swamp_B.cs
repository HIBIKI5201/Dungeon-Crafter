using UnityEngine;
using DCFrameWork.Enemy;
namespace DCFrameWork
{
    abstract public class Swamp_B : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out IEnemy enemy))
            {
                AddCondition(enemy);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IEnemy enemy))
            {
                RemoveCondition(enemy);
            }
        }
        protected abstract void AddCondition(IEnemy enemy);
        protected abstract void RemoveCondition(IEnemy enemy);
    }
}
