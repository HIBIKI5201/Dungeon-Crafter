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
    [SerializeField] NavMeshSurface _navMeshSurface;
    private Camera _mainCamera;
    private Vector3 _currentPosition = Vector3.zero;
    private float _prefabHeight;

    void Start()
    {
        _mainCamera = Camera.main;
        _prefabHeight = _prefab.GetComponent<BoxCollider>().size.y;
    }

    void Update()
    {
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        var raycastHitList = Physics.RaycastAll(ray, float.PositiveInfinity, LayerMask.GetMask("Ground")).ToList();
        if (raycastHitList.Any())
        {
            var hit = raycastHitList.OrderByDescending(x => x.collider.gameObject.transform.position.y).FirstOrDefault();
            _currentPosition = hit.point;
            _clickPointPrefab.transform.position = _currentPosition;
            _currentPosition.y = (int)(_currentPosition.y + hit.normal.y / 2) + 0.5f;
            _currentPosition.x = (int)(_currentPosition.x + hit.normal.x / 2) + 0.5f * Mathf.Sign(_currentPosition.x);
            _currentPosition.z = (int)(_currentPosition.z + hit.normal.z / 2) + 0.5f * Mathf.Sign(_currentPosition.z);
            _clickGridPrefab.transform.position = _currentPosition;
        }
        if (Input.GetMouseButtonDown(0))
        {
            var obj = Instantiate(_prefab, _currentPosition, Quaternion.identity);
            obj.transform.SetParent(_navMeshSurface.gameObject.transform);
            _navMeshSurface.BuildNavMesh();
        }
    }
}
