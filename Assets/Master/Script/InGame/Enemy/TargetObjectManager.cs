using DCFrameWork.Enemy;
using UnityEngine;

namespace DCFrameWork
{
    public class TargetObjectManager : MonoBehaviour
    {
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.TryGetComponent(out IEnemy fightable))
            {    
                fightable.DeathBehaviour();
                WaveManager.EnemyDeathCount();
            }
        }

    }
}
