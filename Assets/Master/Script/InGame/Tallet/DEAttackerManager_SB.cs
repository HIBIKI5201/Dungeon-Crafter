using DCFrameWork.DefenseEquipment;
using DCFrameWork.Enemy;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class DEAttackerManager_SB : DefenseEquipmentManager_B
{
    protected List<EnemyManager_B> _enemyList;

    protected virtual List<EnemyManager_B> TargetSelect()
    {
        return _enemyList.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).Take(1).ToList();
    }

    protected abstract void Attack();

    protected void TargetsAddDamage(List<EnemyManager_B> enemies, float damage)
    {
        foreach (var enemy in enemies)
        {
            enemy.HitDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyManager_B>(out var enemyManager))
        {
            _enemyList.Add(enemyManager);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EnemyManager_B>(out var enemyManager))
        {
            _enemyList.Remove(enemyManager);
        }
    }
}
