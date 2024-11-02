using DCFrameWork.DefenseEquipment;
using DCFrameWork.Enemy;
using DCFrameWork.MainSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTurretManager : DEAttackerManager_SB<DefenseEquipmentData_B>
{
    const float _interval = 1;
    float _timer = 0;
    bool _isPaused = false;
    protected override void Think() //UpDate ‚Æ“¯‹`
    {
        if (!_isPaused)
        {
            var attackRate = 1 / _rate * Time.deltaTime;
            _timer += attackRate;
            if (_timer > _interval)
            {
                Attack();
                _timer = 0;
            }
        }
    }
    protected override void LoadSpecificData(DefenseEquipmentData_B data)
    {
        var coll = GetComponent<SphereCollider>();
        coll.radius = _range;
    }
    protected override void Attack()
    {
        var criticalPoint = Random.Range(0, 100);
        var targetSelect = TargetSelect();
        Debug.Log(targetSelect);
        TargetsAddDamage(targetSelect, criticalPoint <= _critical ? _attack * 3 : _attack);
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
