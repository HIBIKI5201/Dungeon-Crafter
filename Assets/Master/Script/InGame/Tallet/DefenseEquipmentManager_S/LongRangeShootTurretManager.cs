using DCFrameWork.Enemy;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

namespace DCFrameWork.DefenseEquipment
{
    public class LongRangeShootTurretManager : DEShooterManager_SB<LongRangeShooterData>
    {
        float _timer = 0;
        bool _isPaused = false;
        [SerializeField] LayerMask _enemyLayer;
        [SerializeField] float _raySize = 5;
        

        protected override void Start_S()
        {
            _timer = Time.time;
        }
        protected override void Think() //UpDate ‚Æ“¯‹`
        {
            if (_isPaused)
                _timer += Time.deltaTime;

            if (Time.time > (1 / Rate) + _timer && _enemyList.Count > 0)
            {
                EnemyAttack();
                _timer = Time.time;
            }
        }
        protected override (GameObject Obj, IFightable Interface) TargetSelect()
        {
            return _enemyList.OrderByDescending(x => Vector3.Distance(transform.position, x.Obj.transform.position)).ToList().FirstOrDefault();
        }
        protected override void EnemyAttack()
        {
            var criticalPoint = Random.Range(0, 100);
            var targetSelect = TargetSelect();
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

            TurretRotate(targetSelect.Obj.transform);
            _anim.SetTrigger("Attack");
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
