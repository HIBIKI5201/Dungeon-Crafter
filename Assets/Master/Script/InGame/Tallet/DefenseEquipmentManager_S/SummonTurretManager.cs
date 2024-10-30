using DCFrameWork.DefenseEquipment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonTurretManager : DEEntityManager_SB<DefenseEquipmentData_B>
{
    const float _interval = 1;
    float _timer = 0;
    bool _isPaused = false;
    [SerializeField] Transform _summonPosition;
    protected override void Think() //UpDate ‚Æ“¯‹`
    {
        if (!_isPaused)
        {
            var summonRate = 1 / _rate * Time.deltaTime;
            _timer += summonRate;
            if (_timer > _interval)
            {
                _timer = 0;
                Summon(_summonPosition.position);
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
