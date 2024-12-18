using DCFrameWork.Enemy;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public class MiddleRangeShootTurretManager : DEShooterManager_SB<DefenseEquipmentData_B>
    {
        float _timer = 0;
        bool _isPaused = false;

        protected override void Start_S()
        {
            _timer = Time.time;
        }
        protected override void Think() //UpDate �Ɠ��`
        {
            if (_isPaused)
                _timer += Time.deltaTime;

            if (Time.time > 1 / Rate + _timer && _enemyList.Count > 0)
            {
                EnemyAttack();
                _timer = Time.time;
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