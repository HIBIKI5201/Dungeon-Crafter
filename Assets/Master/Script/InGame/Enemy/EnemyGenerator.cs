using DCFrameWork.MainSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace DCFrameWork.Enemy
{
    public class EnemyGenerator : MonoBehaviour
    {
        public ObjectPool<IEnemy> objectPoolNormal;

        [SerializeField]
        EnemyElement[] _objects;

        [SerializeField]
        private Transform _spawnPos;

        public Transform SpawnPos { get { return _spawnPos; } }

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

        Dictionary<EnemyKind, ObjectPool<IEnemy>> _enemyPools = new();

        private void Start()
        {
            foreach (var enemy in _objects)
            {
                var a = ObjectPooling(enemy.obj);
                _enemyPools.Add(enemy.kind, a);
            }
           
            StartCoroutine(Generate());
        }

        private ObjectPool<IEnemy> ObjectPooling(GameObject obj)
        {
            ObjectPool<IEnemy> objPool = null;
            return objPool = new ObjectPool<IEnemy>(
            () =>
            {
                var spawnedEnemy = Instantiate(obj, _spawnPos.position, Quaternion.identity, transform);
                var healthBar = Instantiate(_healthBar, _canvas.transform);
                healthBar.transform.SetParent(_canvas.transform);
                var enemy = spawnedEnemy.GetComponent<IEnemy>();
                enemy.StartByPool(healthBar.GetComponent<EnemyHealthBarManager>(), _targetPos.position);
                return enemy;
            },
           target =>
           {
               target.Initialize(_spawnPos.position, _targetPos.position, () => objPool.Release(target));
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

        IEnumerator Generate()
        {
            yield return null;
            while (true)
            {
                _enemyPools[0].Get();
                yield return FrameWork.PausableWaitForSecond(_spawnInterval);
            }

        }

    }

    [Serializable]
    public struct EnemyElement
    {
        public GameObject obj;
        public EnemyKind kind;
    }

    public enum EnemyKind
    {
        Normal,
        Defense,
        Lead,
        Buff,
        Boss,
        Fly
    }
}