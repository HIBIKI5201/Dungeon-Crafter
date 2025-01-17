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
            _bulletPos = _bullet.transform.position;
        }
        protected override void Think() //UpDate と同義
        {
            if (_isPaused)
                _timer += Time.deltaTime;

            if (Time.time > 1 / Rate + _timer && _enemyList.Count > 0)
            {
                EnemyAttack();
                _timer = Time.time;
                _isShoot = true;
            }
            if (!_isPaused && _isShoot)
            {
                BulletShoot();
            }
        }

        void BulletShoot()
        {
            _shootTimer += Time.deltaTime;
            var dir = _enemyPos - _bulletPos;
            var speed = dir / _hitTime;
            _bullet.transform.forward = dir;
            _bullet.transform.position = _bulletPos + speed * _shootTimer;
            if (_shootTimer >= _hitTime)
            {
                _isShoot = false;
                _bullet.transform.position = _bulletPos;
                _bullet.transform.eulerAngles = _turretModel.transform.eulerAngles + new Vector3(90, 0, 0);
                _shootTimer = 0;
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