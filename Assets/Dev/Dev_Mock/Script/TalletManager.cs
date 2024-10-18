using System.Collections.Generic;
using UnityEngine;

public class TalletManager : MonoBehaviour
{
    List<EnemyManager> enemyList = new();

    LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    private void Update()
    {
        if (enemyList.Count > 0)
        {
            EnemyAttack(enemyList[0]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyManager>(out var enemyManager))
        {
            enemyList.Add(enemyManager);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EnemyManager>(out var enemyManager))
        {
            TargetLost(enemyManager);
        }
    }

    private void EnemyAttack(EnemyManager enemy)
    {
        if (enemy)
        {
            enemy.AddDamage(100 * Time.deltaTime);
            lineRenderer.SetPosition(1, enemy.transform.position);
        }
        else
        {
            TargetLost(enemy);
        }
    }

    private void TargetLost(EnemyManager enemy)
    {
        enemyList.Remove(enemy);
        lineRenderer.SetPosition(1, transform.position);
    }
}
