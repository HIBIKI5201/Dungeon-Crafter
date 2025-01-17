using DCFrameWork.MainSystem;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace DCFrameWork.Enemy
{
    public class LeadEnemyManager : EnemyManager_B<EnemyStateData>
    {

        [SerializeField]
        private GameObject _defenseEnemyPrefab;

        private float _stoppMovingTime = 0.3f;
        private float _coolTime = 1.5f;

        [SerializeField]
        private float _summonRange = 1;

        
        private Canvas _canvas;

        [SerializeField]
        GameObject _healthBar;

        Transform _targetPos;


        protected override void Start_S()
        {

            _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            _targetPos = GameObject.Find("TargetPos").GetComponent<Transform>();
            InstantiateDefenseEnemy();
        }
       
        void InstantiateDefenseEnemy()
        {
            StartCoroutine(InstantiateEnemy());
        }

        IEnumerator InstantiateEnemy()
        {
            while (true)
            {
                var defense = Instantiate(_defenseEnemyPrefab, gameObject.transform.position + gameObject.transform.forward * _summonRange, Quaternion.identity);
                var enemy = defense.GetComponent<IEnemy>();
                var healthBar = Instantiate(_healthBar, _canvas.transform);
                healthBar.transform.SetParent(_canvas.transform);
                enemy.StartByPool(healthBar.GetComponent<EnemyHealthBarManager>());
                enemy.GotoTargetPos(_targetPos.position);
                var enemy2 = GetComponent<IEnemy>();
                enemy2.StopEnemy(_stoppMovingTime);
                yield return FrameWork.PausableWaitForSecond(_coolTime);
            }
            
           

        }




        protected override void DeathBehaviour()
        {
            base.DeathBehaviour();
        }


        protected override void LoadSpecificnData(EnemyStateData data)
        {


        }



        protected override void Pause()
        {
            throw new System.NotImplementedException();
        }

        protected override void Resume()
        {
            throw new System.NotImplementedException();
        }


    }
}