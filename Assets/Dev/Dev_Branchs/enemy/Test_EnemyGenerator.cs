using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using System.Linq;
using Unity.VisualScripting;


namespace DCFrameWork.Enemy
{
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

        Dictionary<GameObject,GameObject> _objectsDict = new();
        private void Start()
        {
            ObjectPooling();
           
        }


        void ObjectPooling()
        {
            
            objectPool = new ObjectPool<GameObject>(() =>
            {
                float[] items = new float[_objects.Count];
                items[0] = 20;
                items[1] = 80;
                int result = (int)ChooseNum(items);
                var spawnedEnemy = Instantiate(_objects[result], _spawnPos.position, Quaternion.identity, transform);
                var agent = spawnedEnemy.GetComponent<NavMeshAgent>();
                if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
                {
                    agent.SetDestination(_target.position);
                }
                var pooledSpawnedObj = spawnedEnemy.AddComponent<Test_ObjectPool>();
                pooledSpawnedObj.objectPool = objectPool;
                GameObject healthBar = Instantiate(_heathBar, _canvas.transform);
                _objectsDict.Add(spawnedEnemy, healthBar);
                return spawnedEnemy;
            },
           target =>
           {
               target.SetActive(true);
               _objectsDict[target].SetActive(true);
               target.transform.position = _spawnPos.position;
               var agent = target.GetComponent<NavMeshAgent>();
               if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
               {
                   agent.SetDestination(_target.position);
               }
           },
           target =>
           {
               target.SetActive(false);
               _objectsDict[target].SetActive(false);
           },
           target =>
           {
               Destroy(target);
               Destroy(_objectsDict[target]);
               _objectsDict.Remove(target);
           },
           true, 10, 1000);

            StartCoroutine(Generate());
        }


        float ChooseNum(float[] floats)
        {
            float total = floats.Sum();

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


        IEnumerator Generate()
        {
            yield return null;
            while (true)
            {
                objectPool.Get();
                yield return new WaitForSeconds(0.8f);
            }

        }

    }
}
