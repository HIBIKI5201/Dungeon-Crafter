using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class DefenseEquipmentManager_B : MonoBehaviour
{
    [SerializeField]
    protected DefenseEquipmentData_B _data;

    protected int Level;
    protected List<EnemyManager_B> _enemyList;

    private void Start()
    {
        if (_data is null)
            Debug.Log("ÉfÅ[É^Ç™Ç†ÇËÇ‹ÇπÇÒ");
    }

    protected virtual List<EnemyManager_B> TargetSelect()
    {
        return _enemyList.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).Take(1).ToList();
    }

    protected abstract void Attack();

    protected void TargetsAddDamage(List<EnemyManager_B> enemies)
    {
        foreach (var enemy in enemies)
        {
            enemy.HitDamage(CalcDamage(_data.Attack));
        }
    }

    private float CalcDamage(float baseDamage)
    {
        float totalDamage = baseDamage;
        return totalDamage;
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