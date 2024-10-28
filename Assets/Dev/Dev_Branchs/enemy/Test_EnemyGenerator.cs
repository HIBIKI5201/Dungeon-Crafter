using DCFrameWork.Enemy;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Test_EnemyGenerator : MonoBehaviour
{
    public ObjectPool<GameObject> objectPool;

    [SerializeField]
    List<GameObject> _objects;

    [SerializeField]
    private Transform _spawnPos;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private GameObject _heathBar;

   
    private void Awake()
    {

        objectPool = new ObjectPool<GameObject>(() =>
        {
            float[] items = new float[_objects.Count];
            items[0] = 20;
            items[1] = 80;
            var result = ChooseNum(items);
            var spawnedEnemy = Instantiate(_objects[(int)result], _spawnPos.position, Quaternion.identity, transform);
            NavMeshAgent agent = spawnedEnemy.GetComponent<NavMeshAgent>();
            if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(_target.position);
            }
            var pooledSpawnedObj = spawnedEnemy.AddComponent<Test_ObjectPool>();
            pooledSpawnedObj.objectPool = objectPool;
            return spawnedEnemy;
        },
        target =>
        {
            target.gameObject.SetActive(true);
        },
        target =>
        {
            target.gameObject.SetActive(false);
        },
        target =>
        {
            Destroy(target);
        },
        true, 10, 100);

        Debug.Log("awake");
        
    }

    


    float ChooseNum(float[] floats)
    {
        float total = 0;

        foreach (var item in floats)
        {
            total += item;
        }

        var randomNum = Random.value * total;

        for (int i = 0; i < floats.Length; i++)
        {
            if (randomNum < floats[i])
            {
                return i;
            }
            else
            {
                randomNum -= floats[i];
            }

           
        }

        return floats.Length - 1;

    }

 

}
