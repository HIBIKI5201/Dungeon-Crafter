using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_EnemyGenerator2 : MonoBehaviour
{
    Test_EnemyGenerator gene;


    private void Start()
    {
        gene = GetComponent<Test_EnemyGenerator>();

        StartCoroutine(Generate());
        
    }

    IEnumerator Generate()
    {
        while (true)
        {
            gene.objectPool.Get();
            float waitTime = UnityEngine.Random.Range(1, 4);
            yield return new WaitForSeconds(waitTime);
        }

    }
}
