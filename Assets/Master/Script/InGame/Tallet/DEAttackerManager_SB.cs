using DCFrameWork.DefenseEquipment;
using DCFrameWork.Enemy;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class DEAttackerManager_SB<Data> : DefenseEquipmentManager_B<Data> where Data : DefenseEquipmentData_B
{
    protected List<EnemyManager_B<EnemyData_B>> _enemyList;

    protected virtual List<EnemyManager_B<EnemyData_B>> TargetSelect() =>
        _enemyList.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).Take(1).ToList();
    

    protected abstract void Attack();

    protected void TargetsAddDamage(List<EnemyManager_B<EnemyData_B>> enemies, float damage)
    {
        foreach (var enemy in enemies)
        {
            enemy.HitDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyManager_B<EnemyData_B>>(out var enemyManager))
        {
            _enemyList.Add(enemyManager);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EnemyManager_B<EnemyData_B>>(out var enemyManager))
        {
            _enemyList.Remove(enemyManager);
        }
    }
}
