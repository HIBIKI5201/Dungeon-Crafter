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


        [SerializeField]
        EnemyElement[] _objects;

        [SerializeField]
        private Transform[] _spawnPos;

        public Transform[] SpawnPos { get { return _spawnPos; } }

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

        Dictionary<EnemyKind, GameObject> _enemyObjs = new();

        public List<ObjectPool<IEnemy>> _nowPool = new();

        public WaveData _waveData;
        private void Start()
        {
            Waving();
        }

        public void Waving()
        {
            foreach (var enemy in _objects)
            {
                _enemyObjs.Add(enemy.kind, enemy.obj);
            }

            foreach (var a in _waveData._spawnData)
            {
                if (_enemyObjs.ContainsKey(a._enemyType))
                {
                    _nowPool.Add(ObjectPooling(_enemyObjs[a._enemyType], DecisionSpawnPoint(a._spawnPoint)));
                }
            }

            StartCoroutine(Generate());
        }

        private ObjectPool<IEnemy> ObjectPooling(GameObject obj, Transform trm)
        {
            ObjectPool<IEnemy> objPool = null;
            return objPool = new ObjectPool<IEnemy>(
            () =>
            {
                var spawnedEnemy = Instantiate(obj, trm.position, Quaternion.identity, transform);
                var healthBar = Instantiate(_healthBar, _canvas.transform);
                healthBar.transform.SetParent(_canvas.transform);
                var enemy = spawnedEnemy.GetComponent<IEnemy>();
                enemy.StartByPool(healthBar.GetComponent<EnemyHealthBarManager>(), _targetPos.position);
                return enemy;
            },
           target =>
           {
               target.Initialize(trm.position, _targetPos.position, () => objPool.Release(target));
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
                foreach (var op in _nowPool)
                {
                    op.Get();
                }

                yield return FrameWork.PausableWaitForSecond(_spawnInterval);
            }

        }

        Transform DecisionSpawnPoint(int i)
        {
            Transform trm;

            if (i == 0)
            {
                trm = _spawnPos[UnityEngine.Random.Range(0, _spawnPos.Length)];
            }
            else 
            {
                trm = _spawnPos[i - 1];
            }
            
            return trm;
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