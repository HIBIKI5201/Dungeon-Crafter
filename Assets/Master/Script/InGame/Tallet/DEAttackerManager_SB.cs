using DCFrameWork.DefenseEquipment;
using DCFrameWork.Enemy;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class DEAttackerManager_SB<Data> : DefenseEquipmentManager_B<Data> where Data : DefenseEquipmentData_B
{
    protected List<GameObject> _enemyList = new();

    protected virtual List<GameObject> TargetSelect()
    {
        return _enemyList.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).Take(1).ToList();
    }


    protected abstract void Attack();

    protected void TargetsAddDamage(List<GameObject> enemies, float damage)
    {
        foreach (var enemy in enemies)
        {
            if (enemy.TryGetComponent(out IFightable component))
                component.HitDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.TryGetComponent<IFightable>(out _))
        {
            Debug.Log("enter the" + other.gameObject.name);
            _enemyList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _enemyList.Remove(other.gameObject);
    }
}
