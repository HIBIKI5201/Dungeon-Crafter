using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace DCFrameWork.Enemy
{
    public class EnemyGenerator : MonoBehaviour
    {
        public ObjectPool<GameObject> objectPool;

        [SerializeField]
        List<EnemyGenerateData> _objects = new();

        [SerializeField]
        private Transform _spawnPos;

        [SerializeField]
        private Transform _targetPos;

        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private GameObject _healthBar;

        private readonly Dictionary<GameObject, GameObject> _objectsDict = new();

        [SerializeField]
        private int _defaultValue = 1;
        [SerializeField]
        private int _maxValue = 100;

        [SerializeField]
        private float _spawnInterval = 3f;

        private void Start()
        {
            ObjectPooling();
        }

        private void Update()
        {
            foreach (var obj in _objectsDict.Keys)
            {
                FollowTarget(obj, _objectsDict[obj]);
            }

        }
        void ObjectPooling()
        {

            objectPool = new ObjectPool<GameObject>(
            () =>
            {
                int resultIndex = ChooseNum(_objects.Select(o => o.SpawnChance));
                var spawnedEnemy = Instantiate(_objects[resultIndex].EnemyPrefab, _spawnPos.position, Quaternion.identity, transform);
                var healthBar = Instantiate(_healthBar, _canvas.transform);
                _objectsDict.Add(spawnedEnemy, healthBar);
                healthBar.transform.SetParent(_canvas.transform);
                var enemy = spawnedEnemy.GetComponent<IEnemy>();
                enemy.StartByPool(healthBar.GetComponentInChildren<EnemyHealthBarManager>(), _targetPos.position);
                return spawnedEnemy;
            },
           target =>
           {
               target.SetActive(true);
               _objectsDict[target].SetActive(true);
               var manager = target.GetComponent<IEnemy>();
               manager.DeathAction = () => objectPool.Release(target);
               target.transform.position = _spawnPos.position;
               manager.Initialize(_targetPos.position);
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
           true, _defaultValue, _maxValue);

            StartCoroutine(Generate());
        }

        int ChooseNum(IEnumerable<int> chances)
        {
            float total = chances.Sum();
            var randomNum = Random.Range(1, total + 1);

            for (int i = 0; i < chances.Count(); i++)
            {
                int element = chances.ElementAt(i);
                if (randomNum < element)
                {
                    return i;
                }
                else
                {
                    randomNum -= element;
                }

            }

            return chances.ElementAt(0);

        }

        public void FollowTarget(GameObject target, GameObject hpBar)
        {

            Vector3 cameraPos = Camera.main.transform.position;
            Vector3 towards = target.transform.position + new Vector3(target.transform.position.x - cameraPos.x, 0, target.transform.position.z - cameraPos.z).normalized;
            Vector2 screenPos = Camera.main.WorldToScreenPoint(towards);
            hpBar.transform.position = screenPos;
        }
        IEnumerator Generate()
        {
            yield return null;
            while (true)
            {
                objectPool.Get();
                yield return new WaitForSeconds(_spawnInterval);
            }

        }
    }

    [Serializable]
    public struct EnemyGenerateData
    {
        public GameObject EnemyPrefab;
        public int SpawnChance;
    }
}