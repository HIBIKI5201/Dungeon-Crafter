using UnityEngine;
namespace DCFrameWork.Enemy
{
    public class NormalEnemyManager : EnemyManager_B<EnemyData_B>
    {
        EnemyHealthBarManager _enemyHealthBarManager;
        public Vector3 healthBarOffset = new Vector3(0,1,0);   
        protected override void Init_S()
        {
           
        }

        private void Update()
        {
            _enemyHealthBarManager?.FollowTarget(transform.position + healthBarOffset);
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
