using DCFrameWork.MainSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.InputManagerEntry;

namespace DCFrameWork.Enemy
{
    public class EnemyGenerator : MonoBehaviour
    {


        [SerializeField]
        EnemyElement[] _objects;

        [SerializeField]
        private Transform[] _spawnPos;

        public Transform[] SpawnPos { get => _spawnPos; } 
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

        public Dictionary<EnemyKind, ObjectPool<IEnemy>> _dict = new();

        [SerializeField]
        PlayerManager _playerManager;

        [SerializeField]
        PhaseManager _phaseData;

        private void Start()
        {


        }



        public void Initialize()
        {

            foreach (var obj in _objects)
            {
                _dict.Add(obj.kind, ObjectPooling((obj.obj), _spawnPos[0].position, obj.kind));

            }
        }


        public void Waving(PhaseData waveData)
        {
            var kinds = _objects.Select(e => e.kind);
            StartCoroutine(Generate(waveData.SpawnData));
        }

        private ObjectPool<IEnemy> ObjectPooling(GameObject obj, Vector3 initPosition, EnemyKind kind)
        {
            ObjectPool<IEnemy> objPool = null;
            return objPool = new ObjectPool<IEnemy>(
            () =>
            {
                var spawnedEnemy = Instantiate(obj, initPosition, Quaternion.identity, transform);
                var healthBar = Instantiate(_healthBar, _canvas.transform);
                healthBar.transform.SetParent(_canvas.transform);
                var enemy = spawnedEnemy.GetComponent<IEnemy>();
                enemy.StartByPool(healthBar.GetComponent<EnemyHealthBarManager>());
                return enemy;
            },

           actionOnGet: target =>
           {

           },
           actionOnRelease: target =>
           {
               target.DeathBehaviour();
               if (target.CurrentHealth <= 0)
               {
                   CollectionSystem.AddEnemyKilCount(kind);
                   _playerManager.ChangeGold(target.DropGold);
                   _playerManager.AddEXP(target.DropEXP);

               }
               _phaseData.EnemyDeathCount();
               target.DeathAction = null;

           },
           target =>
           {
               target.Destroy();
           },
           true, _defaultValue, _maxValue);
        }

        IEnumerator Generate(EnemySpawnData[] data)
        {
            var spawnQueue = new Queue<EnemySpawnData>(data.OrderBy(x => x._spawnTime));
            float spawned = 0;
            while (spawnQueue.Count > 0)
            {
                var i = spawnQueue.Dequeue();
                yield return FrameWork.PausableWaitForSecond(i._spawnTime - spawned);
                var enemy = _dict[i._enemyType].Get();
                enemy.Initialize(_spawnPos[0].position, _targetPos.position, i._enemyLevel, i._enemyType);
                enemy.DeathAction = () => _dict[i._enemyType].Release(enemy);
                enemy.ChooseStatus(i._enemyLevel, i._enemyType);
                spawned = i._spawnTime;
            }
        }






        [Serializable]
        public struct EnemyElement
        {
            public GameObject obj;
            public EnemyKind kind;
        }


    }
}