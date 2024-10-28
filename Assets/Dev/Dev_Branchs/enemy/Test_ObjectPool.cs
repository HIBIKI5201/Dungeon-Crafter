using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Test_ObjectPool : MonoBehaviour
{
    public ObjectPool<GameObject> objectPool;

    float time;

    public Test_ObjectPool instance;


    private void Awake()
    {
        instance = new Test_ObjectPool();

    }
    private void Update()
    {
        if(time > 3)
        {
            time = 0;
            objectPool.Release(gameObject);
        }

        time += Time.deltaTime;
    }
    
}
