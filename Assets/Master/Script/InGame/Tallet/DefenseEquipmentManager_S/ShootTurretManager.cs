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
    protected override void Attack()
    {
        Debug.Log("Attack");
        TargetsAddDamage(TargetSelect(), _attack);
    }

    protected override void Pause()
    {
        _isPaused = true;
    }

    protected override void Resume()
    {
        _isPaused = false;
    }
    public class Bullet : MonoBehaviour, IPausable
    {
        bool _isPause = false;
        public void Pause()
        {
            _isPause = true;
        }

        public void Resume()
        {
            _isPause = false;
        }
    }
}
