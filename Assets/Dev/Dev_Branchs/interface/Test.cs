using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    [SerializeField] UnityEvent uniev;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            uniev.Invoke();
        }
    }
}
