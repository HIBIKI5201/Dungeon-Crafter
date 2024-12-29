using DCFrameWork.Enemy;
using Unity.AppUI.UI;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace DCFrameWork.DefenseEquipment
{
    public class ShortRangeShootTurretManager : DEShooterManager_SB<DefenseEquipmentData_B>
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
        protected override void EnemyAttack()
        {
            var criticalPoint = Random.Range(0, 100);
            var targetSelect = TargetSelect();
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
