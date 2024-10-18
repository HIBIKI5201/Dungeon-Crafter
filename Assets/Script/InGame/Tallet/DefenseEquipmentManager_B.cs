using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class DefenseEquipmentManager_B : MonoBehaviour
{
    [SerializeField]
    protected DefenseEquipmentData_B _data;

    protected int Level;
    protected List<EnemyManager_B> _enemyList;

    protected virtual List<EnemyManager_B> TargetSelect()
    {
        return _enemyList.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).Take(1).ToList();
    }

    protected abstract void Attack();
}
