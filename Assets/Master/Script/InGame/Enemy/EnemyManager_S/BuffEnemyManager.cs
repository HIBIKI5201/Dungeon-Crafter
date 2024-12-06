using UnityEngine;

namespace DCFrameWork.Enemy
{
    public class BuffEnemyManager : EnemyManager_B<EnemyData_B, EnemyRiseData>
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.TryGetComponent(out IConditionable cond))
            {
                cond.AddCondition(ConditionType.defensive);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IConditionable cond))
            {
                cond.RemoveCondition(ConditionType.defensive);
            }
        }
        protected override void DeathBehaviour()
        {
            base.DeathBehaviour();
        }


        protected override void LoadSpecificnData(EnemyData_B data)
        {


        }



        protected override void Pause()
        {
            throw new System.NotImplementedException();
        }

        protected override void Resume()
        {
            throw new System.NotImplementedException();
        }

    }
}