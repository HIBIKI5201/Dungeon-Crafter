using DCFrameWork.Enemy;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public class AreaTurretManager : DefenseEquipmentManager_B<DefenseEquipmentData_B>
    {
        bool _isPaused = false;
        float _timer = 0;
        protected List<(GameObject Obj, IFightable Interface)> _enemyList = new();

        protected override void Start_SB()
        {
            _enemyList = new();
        }
        protected override void Think() //UpDate Ç∆ìØã`
        {
            if (_isPaused)
                _timer += Time.deltaTime;

            if (Time.time > 1 / Rate + _timer && _enemyList.Count > 0)
            {
                Debug.Log("çUåÇíÜ");
                _timer = Time.time;
                TargetsAddDamage(_enemyList, Attack);
            }
        }
        protected void TargetsAddDamage(List<(GameObject Obj, IFightable Interface)> enemies, float damage)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (!enemies[i].Interface.HitDamage(damage))
                {
                    _enemyList.Remove(_enemyList.Where(e => e.Interface == enemies[i].Interface).FirstOrDefault());
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