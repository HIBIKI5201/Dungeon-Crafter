using DCFrameWork.Enemy;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public class LongRangeShootTurretManager : DEShooterManager_SB<LongRangeShooterData>
    {
        float _timer = 0;
        bool _isPaused = false;
        [SerializeField] LayerMask _enemyLayer;
        [SerializeField] float _raySize = 5;
        [SerializeField] GameObject _chargeObj;


        protected override void Start_S()
        {
            _timer = Time.time;
        }
        protected override void Think() //UpDate と同義
        {
            if (_isPaused)
                _timer += Time.deltaTime;

            if (Time.time > (1 / Rate) + _timer)
            {
                if (EnemyAttack())
                {
                    _timer = Time.time;
                    BulletShoot(true);
                }
            }
        }

        protected override (GameObject Obj, IFightable Interface) TargetSelect()
        {
            var pos = new Vector3(transform.position.x, 0, transform.position.z);
            var hit = Physics.OverlapSphere(pos, Range * 5);
            IFightable component = null;
            var enemy = hit.OrderByDescending(x => Vector3.Distance(pos, x.gameObject.transform.position)).Where(x => x.TryGetComponent(out component)).FirstOrDefault();
            if (enemy != null)
            {
                return (enemy.gameObject, component);
            }
            return (null, null);
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
            var originPos = new Vector3(transform.position.x, targetSelect.Obj.transform.position.y, transform.position.z);
            var direction = targetSelect.Obj.transform.position - originPos;
            var hits = Physics.BoxCastAll(originPos, Vector3.one * _raySize / 2, direction, Quaternion.identity, Range * 5);
            List<IEnemy> enemyList = new List<IEnemy>();
            foreach (var hitObj in hits)
            {
                if (hitObj.collider.gameObject.TryGetComponent(out IEnemy component))
                    enemyList.Add(component);
            }
            foreach (var enemy in enemyList.OrderBy(enemy => Vector3.Distance(transform.position, enemy.position)).Take(DefenseEquipmentData.PierceCount))
            {
                TargetsAddDamage(enemy, criticalPoint <= Critical ? Attack * 3 : Attack);
            }

            TurretRotate();
            _anim.SetTrigger("Attack");

            return true;
        }
        public void BulletShoot(bool @bool)
        {
            _bullet.SetActive(@bool);
            _chargeObj.SetActive(!@bool);
        }
        protected override void LoadSpecificData(LongRangeShooterData data)
        {
            var bullet = _bullet.transform.localScale;
            bullet.y = data.Range * 0.2f * 2;
            _bullet.transform.localScale = bullet;
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
