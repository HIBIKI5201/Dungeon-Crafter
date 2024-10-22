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
    [SerializeField] GameObject _walls;
    private Camera _mainCamera;
    private Vector3 _currentPosition = Vector3.zero;
    private float _prefabHeight;
    bool _canSet = false;

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
            Debug.DrawRay(_currentPosition , Vector3.down, Color.green, 1f);
            if (_clickGridPrefab.transform.position.y > 2f || !Physics.Raycast(_currentPosition, Vector3.down, 1f, LayerMask.GetMask("Ground")))
            {
                Debug.Log("’u‚¯‚È‚¢");
                Debug.Log(_clickGridPrefab.transform.position.y > 2f);
                Debug.Log(!Physics.Raycast(_currentPosition, Vector3.down, 1f, LayerMask.GetMask("Ground")));
                _canSet = false;
                _clickGridPrefab.SetActive(false);
            }
            else
            {
                _canSet = true;
                _clickGridPrefab.SetActive(true);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (_canSet)
            {
                var obj = Instantiate(_prefab, _currentPosition, Quaternion.identity);
                obj.transform.SetParent(_walls.transform);
                _navMeshSurface.BuildNavMesh();
            }
        }
    }
}
