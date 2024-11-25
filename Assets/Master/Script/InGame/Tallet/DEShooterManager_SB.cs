using DCFrameWork.Enemy;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
namespace DCFrameWork.DefenseEquipment
{
    public abstract class DEShooterManager_SB<Data> : DefenseEquipmentManager_B<Data> where Data : DefenseEquipmentData_B
    {
        protected List<(GameObject Obj, IFightable Interface)> _enemyList = new();
        [SerializeField] GameObject _turret;

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


        protected virtual void Attack()
        {
            var criticalPoint = Random.Range(0, 100);
            var targetSelect = TargetSelect();
            TargetsAddDamage(targetSelect.Interface, criticalPoint <= DefenseEquipmentData.Critical ? DefenseEquipmentData.Attack * 3 : DefenseEquipmentData.Attack);
            TurretRotate(targetSelect.Obj.transform);
        }

        protected virtual void TurretRotate(Transform enemy)
        {
            _turret.transform.forward = enemy.transform.position - _turret.transform.position;
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