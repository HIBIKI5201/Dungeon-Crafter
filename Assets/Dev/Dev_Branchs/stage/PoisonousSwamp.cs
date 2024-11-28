using DCFrameWork.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork
{
    public class PoisonousSwamp : Swamp_B
    {
        [SerializeField] float _damage;
        [SerializeField] float _rate;
        List<EnemyManager_B<EnemyData_B>> _enemies = new List<EnemyManager_B<EnemyData_B>>();
        private void Start()
        {
            StartCoroutine(AddDamage());
        }
        protected override void AddCondition(EnemyManager_B<EnemyData_B> enemy)
        {
            _enemies.Add(enemy);
        }
        protected override void RemoveCondition(EnemyManager_B<EnemyData_B> enemy)
        {
            _enemies.Remove(enemy);
        }
        IEnumerator AddDamage()
        {
            while (true)
            {
                foreach (EnemyManager_B<EnemyData_B> enemy in _enemies)
                {
                    if (enemy.TryGetComponent(out IFightable component))
                    {
                        component.HitDamage(_damage);
                        //Debug.Log("ì≈É_ÉÅÅ[ÉW");
                    }
                }
                yield return new WaitForSeconds(_rate);
            }         
        }
    }
}
