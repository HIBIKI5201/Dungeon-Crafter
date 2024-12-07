using DCFrameWork.Enemy;
using System.Linq;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public class LongRangeShootTurretManager : DEShooterManager_SB<DefenseEquipmentData_B>
    {
        float _timer = 0;
        bool _isPaused = false;

        protected override void Start_S()
        {
            _timer = Time.time;
        }
        protected override void Think() //UpDate ‚Æ“¯‹`
        {
            if (_isPaused)
                _timer += Time.deltaTime;

            if (Time.time > 1 / Rate + _timer && _enemyList.Count > 0)
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
            //var ray = 
            TargetsAddDamage(targetSelect.Interface, criticalPoint <= Critical ? Attack * 3 : Attack);
            TurretRotate(targetSelect.Obj.transform);
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
