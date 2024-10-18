using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private Transform _spawnPos;
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private GameObject _heathBar;
    private void Start()
    {
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        while (true)
        {
            GameObject enemy = Instantiate(_enemyPrefab, _spawnPos.position, Quaternion.identity, transform);
            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            agent.SetDestination(_target.position);
            GameObject healthBar = Instantiate(_heathBar, _canvas.transform);
            EnemyHealthBarManager healthBarManager = healthBar.GetComponent<EnemyHealthBarManager>();
            enemy.GetComponent<EnemyManager>().Init(healthBarManager);
            float waitTime = UnityEngine.Random.Range(1, 4);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
