using UnityEngine;

namespace DCFrameWork.Enemy
{
    public class BuffEreaManager : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IConditionable cond))
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
    }
}
