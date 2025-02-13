﻿using DCFrameWork.Enemy;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public class ShortRangeShootTurretManager : DEShooterManager_SB<ShortRangeData>
    {
        float _timer = 0;
        bool _isPaused = false;
        [SerializeField] Transform _muzzlePos;

        protected override void Start_S()
        {
            _timer = Time.time;
            _bulletPos = _bullet.transform.position;
        }
        protected override void Think() //UpDate と同義
        {
            if (_isPaused)
                _timer += Time.deltaTime;

            if (Time.time > 1 / Rate + _timer)
            {
                if (EnemyAttack())
                {
                    _timer = Time.time;
                    _isShoot = true;
                }
            }
            if (!_isPaused && _isShoot)
            {
                BulletShoot();
            }
        }
        protected override bool EnemyAttack()
        {
            var criticalPoint = Random.Range(0, 100);
            if (TargetSelect() == (null, null))
            {
                return false;
            }
            var targetSelect = TargetSelect();
            _enemyPos = targetSelect.Obj.transform.position;
            TurretRotate();

            var hits = Physics.OverlapSphere(_enemyPos, DefenseEquipmentData.ExplosionRadius * 5);

            foreach (var hit in hits)
            {
                if (hit.gameObject.TryGetComponent(out IEnemy component))
                    TargetsAddDamage(component, criticalPoint <= Critical ? Attack * 3 : Attack);
            }
            _anim.SetTrigger("Attack");
            return true;
        }

        void BulletShoot()
        {
            _bullet.SetActive(true);
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
                _bullet.SetActive(false);
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
