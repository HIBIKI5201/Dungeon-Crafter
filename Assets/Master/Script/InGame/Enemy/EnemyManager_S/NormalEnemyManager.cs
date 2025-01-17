

using DCFrameWork.MainSystem;
using System.Collections;

namespace DCFrameWork.Enemy
{
    public class NormalEnemyManager : EnemyManager_B<EnemyStateData>
    {
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