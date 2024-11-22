using DCFrameWork.Enemy;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace DCFrameWork.DefenseEquipment
{
    public abstract class DEAttackerManager_SB<Data> : DefenseEquipmentManager_B<Data> where Data : DefenseEquipmentData_B
    {
        protected List<(GameObject Obj, IFightable Interface)> _enemyList = new();

        protected override void Start_SB()
        {
            _enemyList = new();
            Start_S();
        }

        protected virtual void Start_S() { }

        protected virtual List<IFightable> TargetSelect()
        {
            return _enemyList.OrderBy(x => Vector3.Distance(transform.position, x.Obj.transform.position)).Select(x => x.Interface).Take(1).ToList();
        }


        protected abstract void Attack();

        protected void TargetsAddDamage(List<IFightable> enemies, float damage)
        {
            foreach (var enemy in enemies)
            {
                if (!enemy.HitDamage(damage))
                {
                    _enemyList.Remove(_enemyList.Where(e => e.Interface == enemy).FirstOrDefault());
                }
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