using DCFrameWork.MainSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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

        private Dictionary<EnemyKind, ObjectPool<IEnemy>> _dict = new();

        private Vector3 _initPosition = new Vector3(-22.5f,0,22.5f);

        private void Start()
        {
           //Initialize();
        }

       

        public void Initialize()
        {

            foreach (var obj in _objects)
            {
                _dict.Add(obj.kind, ObjectPooling(obj.obj));
            }
        }


        public void Waving(WaveData waveData)
        {
            var kinds = _objects.Select(e => e.kind);
            foreach (var a in waveData._spawnData)
            {
                if (kinds.Contains(a._enemyType))
                {
                    StartCoroutine(Generate(a, _dict[a._enemyType]));
                }
            }
        }

        private ObjectPool<IEnemy> ObjectPooling(GameObject obj)
        {
            ObjectPool<IEnemy> objPool = null;
            return objPool = new ObjectPool<IEnemy>(
            () =>
            {
                var spawnedEnemy = Instantiate(obj,_initPosition, Quaternion.identity, transform);
                var healthBar = Instantiate(_healthBar, _canvas.transform);
                healthBar.transform.SetParent(_canvas.transform);
                var enemy = spawnedEnemy.GetComponent<IEnemy>();
                enemy.StartByPool(healthBar.GetComponent<EnemyHealthBarManager>(), _targetPos.position);
                return enemy;
            },
           target =>
           {
               target.Initialize( Vector3.zero, _targetPos.position, () => objPool.Release(target));
           },
           target =>
           {
               target.DeathBehaviour();
               WaveManager.EnemyDeathCount();
           },
           target =>
           {
               target.Destroy();
           },
           true, _defaultValue, _maxValue);
        }

        IEnumerator Generate(EnemySpawnData data, ObjectPool<IEnemy> pool)
        {
            yield return FrameWork.PausableWaitForSecond(data._spawnStartTime);
            float timer = data._spawnEndTime - data._spawnStartTime;
            timer = timer / data._enemyCount;
            int count = data._enemyCount;
            while (count > 0)
            {
                pool.Get().position = DecisionSpawnPoint(data._spawnPoint);
                count--;
                yield return FrameWork.PausableWaitForSecond(timer);
            }

        }

        Vector3 DecisionSpawnPoint(int i)
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
            
            return trm.position;
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