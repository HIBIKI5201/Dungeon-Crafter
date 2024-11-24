using DCFrameWork.Enemy;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public class WeeknessTurretManager : DEWalkerableManager_SB<DefenseEquipmentData_B>
    {
        bool _isPaused = false;
        protected override void Think() //UpDate ‚Æ“¯‹`
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isPaused)
            {
                if (other.TryGetComponent(out IConditionable component))
                {
                    TargetAddCondition(component, ConditionType.weakness);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_isPaused)
            {
                if (other.TryGetComponent(out IConditionable component))
                {
                    TargetRemoveCondition(component, ConditionType.weakness);
                }
            }
        }
        protected override void Pause()
        {
            _isPaused = true;
        }

        protected override void Resume()
        {
            _isPaused = false;
        }

    }
}