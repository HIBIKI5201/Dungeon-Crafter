using DCFrameWork.DefenseEquipment;
using DCFrameWork.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTurretManager : DEWalkerableManager_SB<DefenseEquipmentData_B>
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
                TargetAddCondition(component, ConditionType.slow);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isPaused)
        {
            if (other.TryGetComponent(out IConditionable component))
            {
                TargetRemoveCondition(component, ConditionType.slow);
            }
        }
    }
    protected override void LoadSpecificData(DefenseEquipmentData_B data)
    {
        throw new System.NotImplementedException();
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
