using DCFrameWork.DefenseEquipment;
using DCFrameWork.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTurretManager : DEWalkerableManager_SB<DefenseEquipmentData_B>
{
    const float _interval = 1;
    float _timer = 0;
    bool _isPaused = false;
    [SerializeField] string _enemyTag;
    protected override void Think() //UpDate ‚Æ“¯‹`
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isPaused) 
        {
            if (other.gameObject.tag == _enemyTag)
            {
                TargetAddCondition(other.gameObject.GetComponent<EnemyManager_B<EnemyData_B>>(), ConditionType.slow);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isPaused) 
        {
            if (other.gameObject.tag == _enemyTag)
            {
                TargetRemoveCondition(other.gameObject.GetComponent<EnemyManager_B<EnemyData_B>>(), ConditionType.slow);
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
