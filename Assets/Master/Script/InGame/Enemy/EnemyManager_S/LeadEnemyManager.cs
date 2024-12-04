using DCFrameWork.MainSystem;
using System.Collections;
using UnityEngine;

namespace DCFrameWork.Enemy
{
    public class LeadEnemyManager : EnemyManager_B<EnemyData_B, EnemyRiseData>
    {

        [SerializeField]
        private GameObject _defenseEnemyPrefab;

        private float _stoppMovingTime = 0.3f;
        private float _coolTime = 1.5f;

        [SerializeField]
        private float _summonRange = 1;

        private void Update()
        {
            InstantiateDefenseEnemy();
        }

        void InstantiateDefenseEnemy()
        {
            StartCoroutine(InstantiateEnemy());
        }

        IEnumerator InstantiateEnemy()
        {
            Instantiate(_defenseEnemyPrefab, gameObject.transform.forward.normalized * _summonRange, Quaternion.identity);
            yield return FrameWork.PausableWaitForSecond(_coolTime);

        }




        protected override void DeathBehaviour()
        {
            base.DeathBehaviour();
        }


        protected override void LoadSpecificnData(EnemyData_B data)
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