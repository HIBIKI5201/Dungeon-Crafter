namespace DCFrameWork.Enemy
{
    public class BossEnemyManager : EnemyManager_B<EnemyData_B>
    {
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