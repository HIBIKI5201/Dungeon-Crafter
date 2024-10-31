using DCFrameWork.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class NormalEnemyManager : EnemyManager_B<EnemyData_B>
{

    public ObjectPool<GameObject> objectPool;


    protected override void Init_S()
    {
        
    }

    private void Update()
    {
        
    }

    protected override void LoadSpecificnData(EnemyData_B data) 
    {

    }

    protected override void DeathBehaviour()
    {
        objectPool.Release(gameObject);
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
