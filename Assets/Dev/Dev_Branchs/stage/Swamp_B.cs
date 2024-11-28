using UnityEngine;
using DCFrameWork.Enemy;
namespace DCFrameWork
{
    abstract public class Swamp_B : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log(other.gameObject.name);
            if(other.TryGetComponent(out EnemyManager_B<EnemyData_B> enemy))
            {
                AddCondition(enemy);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out EnemyManager_B<EnemyData_B> enemy))
            {
                RemoveCondition(enemy);
            }
        }
        protected abstract void AddCondition(EnemyManager_B<EnemyData_B> enemy);
        protected abstract void RemoveCondition(EnemyManager_B<EnemyData_B> enemy);
    }
}
