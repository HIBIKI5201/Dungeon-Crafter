using DCFrameWork.DefenseEquipment;
using DCFrameWork.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTalletManager : DEAttackerManager_SB<DefenseEquipmentData_B>
{
    float _timer = 0;
    protected override void Think() //UpDate ‚Æ“¯‹`
    {
        var interval = 1 / _rate * Time.deltaTime;
        _timer += interval;
        if (_timer > 1)
        {
            Attack();
            _timer = 0;
        }
    }
    protected override void Attack()
    {
        Debug.Log("Attack");
    }

    protected override void LoadSpecificData(DefenseEquipmentData_B data)
    {
        throw new System.NotImplementedException();
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
