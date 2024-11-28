using DCFrameWork.Enemy;
using UnityEngine;

namespace DCFrameWork
{
    public class MuddySwamp : Swamp_B
    {
        protected override void AddCondition(EnemyManager_B<EnemyData_B> enemy)
        {
            if(enemy.TryGetComponent(out IConditionable component))
            {
                component.AddCondition(ConditionType.slow);
            }
        }
        protected override void RemoveCondition(EnemyManager_B<EnemyData_B> enemy)
        {
            if (enemy.TryGetComponent(out IConditionable component))
            {
                component.RemoveCondition(ConditionType.slow);
            }
        }
    }
}
