using DCFrameWork.Enemy;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace DCFrameWork.DefenseEquipment
{
    public abstract class DEShooterManager_SB<Data> : DefenseEquipmentManager_B<Data> where Data : DefenseEquipmentData_B
    {
        protected List<(GameObject Obj, IFightable Interface)> _enemyList = new();
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
            _enemyList = new();
            Start_S();
        }

        protected virtual void Start_S() { }

        protected virtual (GameObject Obj, IFightable Interface) TargetSelect()
        {
            return _enemyList.OrderBy(x => Vector3.Distance(transform.position, x.Obj.transform.position)).ToList().FirstOrDefault();
        }


        protected virtual void EnemyAttack()
        {
            var criticalPoint = Random.Range(0, 100);
            var targetSelect = TargetSelect();
            _enemyPos = targetSelect.Obj.transform.position;
            TargetsAddDamage(targetSelect.Interface, criticalPoint <= Critical ? Attack * 3 : Attack);
            TurretRotate(targetSelect.Obj.transform);
            _anim.SetTrigger("Attack");
        }

        protected virtual void TurretRotate(Transform enemy)
        {
            var dir = enemy.transform.position - _turretModel.transform.position;
            dir.y = 0;
            dir.Normalize();

            var targetRotation = Quaternion.LookRotation(dir);

            _turretModel.transform.localRotation = targetRotation;
        }

        protected void TargetsAddDamage(IFightable enemy, float damage)
        {
            if (!enemy.HitDamage(damage))
            {
                _enemyList.Remove(_enemyList.Where(e => e.Interface == enemy).FirstOrDefault());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IFightable>(out var component))
            {
                _enemyList.Add((other.gameObject, component));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var result = _enemyList.Find(e => e.Obj == other.gameObject);
            _enemyList.Remove(result);
        }
    }
}