using DCFrameWork.DefenseEquipment;
using DCFrameWork.Enemy;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public abstract class DEWalkerableManager_SB<Data> : DefenseEquipmentManager_B<Data> where Data : DefenseEquipmentData_B
    {
        private bool _isPaused = false;
        [SerializeField] ConditionType _conditionType;

        protected void TargetAddCondition(IConditionable enemy, ConditionType type)
        {
            enemy.AddCondition(type);
        }

        protected void TargetRemoveCondition(IConditionable enemy, ConditionType type)
        {
            enemy.RemoveCondition(type);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!_isPaused)
            {
                if (other.TryGetComponent(out IConditionable component))
                {
                    TargetAddCondition(component, _conditionType);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_isPaused)
            {
                if (other.TryGetComponent(out IConditionable component))
                {
                    TargetRemoveCondition(component, _conditionType);
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