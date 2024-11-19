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
        public ObjectPool<IEnemy> objectPool;

        [SerializeField]
        List<EnemyGenerateData> _objects = new();

        [SerializeField]
        private Transform _spawnPos;
        
        public Transform SpawnPos {  get { return _spawnPos; } }

        [SerializeField]
        private Transform _targetPos;

        public Transform TargetPos { get { return _targetPos; } }

        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private GameObject _healthBar;

        [SerializeField]
        private int _defaultValue = 1;
        [SerializeField]
        private int _maxValue = 100;

        [SerializeField]
        private float _spawnInterval = 3f;

        private void Start()
        {
            ObjectPooling();
            StartCoroutine(Generate());
        }

        private void ObjectPooling()
        {

            objectPool = new ObjectPool<IEnemy>(
            () =>
            {
                int resultIndex = ChooseNum(_objects.Select(o => o.SpawnChance));
                var spawnedEnemy = Instantiate(_objects[resultIndex].EnemyPrefab, _spawnPos.position, Quaternion.identity, transform);
                var healthBar = Instantiate(_healthBar, _canvas.transform);
                healthBar.transform.SetParent(_canvas.transform);
                var enemy = spawnedEnemy.GetComponent<IEnemy>();
                enemy.StartByPool(healthBar.GetComponent<EnemyHealthBarManager>(), _targetPos.position);
                return enemy;
            },
           target =>
           {
               target.Initialize(_spawnPos.position, _targetPos.position, () => objectPool.Release(target));
           },
           target =>
           {
               target.DeathBehaviour();
           },
           target =>
           {
               target.Destroy();
           },
           true, _defaultValue, _maxValue);
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

            return 0;

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