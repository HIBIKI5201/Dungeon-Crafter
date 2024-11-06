using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System.Linq;
using DCFrameWork.MainSystem;


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
        private Transform _targetPos;

        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private GameObject _healthBar;

        public Dictionary<GameObject,GameObject> _objectsDict = new() ;

        [SerializeField]
        public int _defaultValue = 1;
        [SerializeField]
        public int _maxValue = 100 ;

        [SerializeField]
        public float _spawnInterval = 3f;

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
            
            objectPool = new ObjectPool<GameObject>(() =>
            {
                float[] items = new float[_objects.Count];
                items[0] = 20;
                items[1] = 80;
                int result = (int)ChooseNum(items);
                var spawnedEnemy = Instantiate(_objects[result], _spawnPos.position, Quaternion.identity, transform);
                var healthBar = Instantiate(_healthBar, _canvas.transform);
                _objectsDict.Add(spawnedEnemy, healthBar);
                healthBar.transform.SetParent(_canvas.transform);
                var enemy = spawnedEnemy.GetComponent<IEnemy>();
                enemy.StartByPool(healthBar.GetComponentInChildren<EnemyHealthBarManager>(),_targetPos.position);
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

        public void FollowTarget(GameObject target ,GameObject hpBar)
        {
            
            Vector3 cameraPos = Camera.main.transform.position;
            Vector3 towards = target.transform.position + new Vector3(target.transform.position.x - cameraPos.x, 0, target.transform.position.z - cameraPos.z).normalized ;
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
}
