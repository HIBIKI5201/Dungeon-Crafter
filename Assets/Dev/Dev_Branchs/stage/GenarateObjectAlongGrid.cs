using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenarateObjectAlongGrid : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    private Camera mainCamera;
    private Vector3 currentPosition = Vector3.zero;
    private float _prefabHeight;

    void Start()
    {
        mainCamera = Camera.main;
        _prefabHeight = _prefab.GetComponent<BoxCollider>().size.y;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var raycastHitList = Physics.RaycastAll(ray).ToList();
            if (raycastHitList.Any())
            {

                var distance = Vector3.Distance(mainCamera.transform.position, raycastHitList.First().point);
                var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);

                currentPosition = mainCamera.ScreenToWorldPoint(mousePosition);
                currentPosition.y = _prefabHeight / 2;
                currentPosition.x = (int)(currentPosition.x);
                currentPosition.x = (int)(currentPosition.z);
                Instantiate(_prefab, currentPosition, Quaternion.identity);
            }
        }
    }
}
