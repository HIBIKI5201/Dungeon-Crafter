using DCFrameWork.Enemy;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public class ShortRangeShootTurretManager : DEShooterManager_SB<DefenseEquipmentData_B>
    {
        float _timer = 0;
        bool _isPaused = false;

        protected override void Start_S()
        {
            _timer = Time.time;
            _bulletPos = _bullet.transform.position;
        }
        protected override void Think() //UpDate ‚Æ“¯‹`
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
        protected override void EnemyAttack()
        {
            var criticalPoint = Random.Range(0, 100);
            var targetSelect = TargetSelect();
            _enemyPos = targetSelect.Obj.transform.position;
            TurretRotate(targetSelect.Obj.transform);
            var originPos = new Vector3(transform.position.x, targetSelect.Obj.transform.position.y, transform.position.z);
            var direction = targetSelect.Obj.transform.position - originPos;

            var hits = Physics.SphereCastAll(originPos, Range, direction, Range * 5);

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.TryGetComponent(out IEnemy component))
                    TargetsAddDamage(component, criticalPoint <= Critical ? Attack * 3 : Attack);
            }
            _anim.SetTrigger("Attack");
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
