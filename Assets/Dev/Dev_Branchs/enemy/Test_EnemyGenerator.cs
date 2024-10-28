using DCFrameWork.Enemy;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Test_EnemyGenerator : MonoBehaviour
{
    ObjectPool<GameObject> objectPool;
    [SerializeField]List<GameObject> _objects;
    private void Awake()
    {
       
        
    }

    private void Update()
    {
        RandomEnemy();
    }

    void RandomEnemy()
    {
        float[] items = new float[_objects.Count];
        items[0] = 20;
        items[1] = 80;

        var result = ChooseEnemy(items);
        Debug.Log(result);
    }
    float ChooseEnemy(float[] floats)
    {
        float total = 0;

        foreach (var item in floats)
        {
            total += item;
        }

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

    

}
