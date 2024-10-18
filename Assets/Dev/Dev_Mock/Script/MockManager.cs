using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;

public class MockManager : MonoBehaviour
{
    [SerializeField]
    GameObject tallet;

    [SerializeField]
    NavMeshSurface _navMeshSurface;
    void Start()
    {
        StartCoroutine(MockBehaviour());
    }

    IEnumerator MockBehaviour()
    {
        while (true)
        {
            tallet.SetActive(false);
            _navMeshSurface.BuildNavMesh();
            yield return new WaitForSeconds(5f);
            tallet.SetActive(true);
            _navMeshSurface.BuildNavMesh();
            yield return new WaitForSeconds(5f);
        }
    }
}
