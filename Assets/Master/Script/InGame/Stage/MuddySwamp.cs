using DCFrameWork.Enemy;
using UnityEngine;

namespace DCFrameWork
{
    public class MuddySwamp : Swamp_B
    {
        protected override void AddCondition(IEnemy enemy)
        {
            enemy.AddCondition(ConditionType.slow);
        }
        protected override void RemoveCondition(IEnemy enemy)
        {
            enemy.RemoveCondition(ConditionType.slow);
        }
    }
}
