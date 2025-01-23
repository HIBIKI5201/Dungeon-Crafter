using DCFrameWork.Enemy;
using System.Linq;
using UnityEngine;
namespace DCFrameWork.DefenseEquipment
{
    public abstract class DEShooterManager_SB<Data> : DefenseEquipmentManager_B<Data> where Data : DefenseEquipmentData_B
    {
        [SerializeField] protected GameObject _turretModel;
        [SerializeField] protected Animator _anim;
        protected Vector3 _enemyPos;
        [SerializeField] protected GameObject _bullet;
        [Tooltip("弾が着弾するまでの時間")] protected float _hitTime = 0.2f;
        [Tooltip("弾の初期位置")] protected Vector3 _bulletPos;
        protected float _shootTimer;
        protected bool _isShoot;

        protected override void Start_SB()
        {
            Start_S();
        }

        protected virtual void Start_S() { }

        protected virtual (GameObject Obj, IFightable Interface) TargetSelect()
        {
            var pos = new Vector3(transform.position.x, 0, transform.position.z);
            var hit = Physics.OverlapSphere(pos, Range * 5);
            IFightable component = null;
            var enemy = hit.OrderBy(x => Vector3.Distance(pos, x.gameObject.transform.position)).Where(x => x.TryGetComponent(out component)).FirstOrDefault();
            if (enemy != null)
            {
                return (enemy.gameObject, component);
            }
            return (null, null);
        }


        protected virtual bool EnemyAttack()
        {
            var criticalPoint = Random.Range(0, 100);
            if (TargetSelect() == (null, null))
            {
                return false;
            }
            var targetSelect = TargetSelect();
            _enemyPos = targetSelect.Obj.transform.position;
            TargetsAddDamage(targetSelect.Interface, criticalPoint <= Critical ? Attack * 3 : Attack);
            TurretRotate();
            _anim.SetTrigger("Attack");

            return true;
        }

        protected virtual void TurretRotate()
        {
            var dir = _turretModel.transform.position - _enemyPos;
            dir.y = 0;
            dir.Normalize();

            var targetRotation = Quaternion.LookRotation(dir);

            _turretModel.transform.localRotation = targetRotation;
        }

        protected void TargetsAddDamage(IFightable enemy, float damage)
        {
            enemy.HitDamage(damage);
        }
    }
}