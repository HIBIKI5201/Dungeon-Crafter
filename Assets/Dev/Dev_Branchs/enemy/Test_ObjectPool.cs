using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Test_ObjectPool : MonoBehaviour
{
    public ObjectPool<GameObject> objectPool;

    float time;

 
    private void Update()
    {
        if(time > 5)
        {
            time = 0;
            objectPool.Release(gameObject);
        }

        time += Time.deltaTime;
    }
    
}
