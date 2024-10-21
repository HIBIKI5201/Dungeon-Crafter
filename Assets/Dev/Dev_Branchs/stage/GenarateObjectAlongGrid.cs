using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class GenarateObjectAlongGrid : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] GameObject _clickPointPrefab;
    [SerializeField] GameObject _clickGridPrefab;
    NavMeshSurface _navMeshSurface;
    private Camera _mainCamera;
    private Vector3 _currentPosition = Vector3.zero;
    private float _prefabHeight;

    void Start()
    {
        _mainCamera = Camera.main;
        _prefabHeight = _prefab.GetComponent<BoxCollider>().size.y;
        _navMeshSurface = FindAnyObjectByType<MockManager>().GetComponent<NavMeshSurface>();
    }

    void Update()
    {
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        var raycastHitList = Physics.RaycastAll(ray,float.PositiveInfinity,LayerMask.GetMask("Ground")).ToList();
        foreach(var raycastHit in raycastHitList)
        {
            Debug.Log(raycastHit.collider.gameObject.name);
        }
        if (raycastHitList.Any())
        {
            _currentPosition = raycastHitList.OrderByDescending(x => x.collider.gameObject.transform.position.y).FirstOrDefault().point;
            //_currentPosition = raycastHitList.First().point;
            _clickPointPrefab.transform.position = _currentPosition;
            
            _currentPosition.y = Mathf.Ceil(_currentPosition.y);
            _currentPosition.y += _prefabHeight / 2;
            _currentPosition.x = (int)(_currentPosition.x) + 0.5f * Mathf.Sign(_currentPosition.x);
            _currentPosition.z = (int)(_currentPosition.z) + 0.5f * Mathf.Sign(_currentPosition.z);
            _clickGridPrefab.transform.position = _currentPosition;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(_prefab, _currentPosition, Quaternion.identity);
            _navMeshSurface.BuildNavMesh();
        }
    }
}
