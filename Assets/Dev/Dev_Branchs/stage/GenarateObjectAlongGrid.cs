using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class GenarateObjectAlongGrid : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] GameObject _clickPointPrefab;
    [SerializeField] GameObject _clickGridPrefab;
    [SerializeField] NavMeshSurface _navMeshSurface;
    [SerializeField] GameObject _walls;
    [SerializeField] Transform _startPos;
    [SerializeField] Transform _targetPos;
    [SerializeField] StageManager _stageManager;
    private NavMeshPath _path;
    private Camera _mainCamera;
    private Vector3 _currentPosition = Vector3.zero;
    private float _prefabHeight;
    bool _canSet = false;

    void Start()
    {
        _path = new NavMeshPath();
        _mainCamera = Camera.main;
        _prefabHeight = _prefab.GetComponent<BoxCollider>().size.y;
    }

    void Update()
    {
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        var raycastHitList = Physics.RaycastAll(ray, float.PositiveInfinity, LayerMask.GetMask("Ground")).ToList();
        var beforeCurrentPos = _currentPosition;
        if (raycastHitList.Any())
        {
            var hit = raycastHitList.Where(x => x.collider.gameObject != _clickGridPrefab).OrderByDescending(x => x.collider.gameObject.transform.position.y).FirstOrDefault();
            _currentPosition = hit.point;
            _clickPointPrefab.transform.position = _currentPosition;
            //グリッドの計算式
            _currentPosition.y = (int)((_currentPosition.y + hit.normal.y / 2) / 5) * 5 + 2.5f;
            _currentPosition.x = (int)((_currentPosition.x + hit.normal.x / 2) / 5) * 5 + 2.5f * Mathf.Sign(_currentPosition.x);
            _currentPosition.z = (int)((_currentPosition.z + hit.normal.z / 2) / 5) * 5 + 2.5f * Mathf.Sign(_currentPosition.z);
            _clickGridPrefab.transform.position = _currentPosition;
            _stageManager.CheckStage(_currentPosition);
            Debug.DrawRay(_currentPosition, Vector3.down, Color.green, 1f);
            if (_clickGridPrefab.transform.position.y > 8f || !Physics.Raycast(_currentPosition, Vector3.down, 5f, LayerMask.GetMask("Ground")))
            {
                _canSet = false;
                _clickGridPrefab.SetActive(false);
            }
            else
            {
                _canSet = true;
                _clickGridPrefab.SetActive(true);
            }
        }
        NavMesh.CalculatePath(_startPos.position, _targetPos.position, NavMesh.AllAreas, _path);
        for (int i = 0; i < _path.corners.Length - 1; i++)
        {
            Debug.DrawLine(_path.corners[i], _path.corners[i + 1], Color.red);
            if (Physics.Raycast(_path.corners[i], _path.corners[i + 1] - _path.corners[i], out RaycastHit hit, Vector3.Distance(_path.corners[i], _path.corners[i + 1]), LayerMask.GetMask("Ground")))
            {
                _clickGridPrefab.GetComponent<MeshRenderer>().material.color = Color.red;
                _canSet = false;
                break;
            }
            else
            {
                _clickGridPrefab.GetComponent<MeshRenderer>().material.color = Color.blue;
            }
            if (hit.collider != null)
                Debug.Log(hit.collider.gameObject.name);
        }
        if (beforeCurrentPos != _currentPosition && _currentPosition.y < 1)
            _navMeshSurface.BuildNavMesh();
        if (Input.GetMouseButtonDown(0))
        {
            if (_canSet)
            {
                var obj = Instantiate(_prefab, _currentPosition, Quaternion.identity);
                obj.transform.SetParent(_walls.transform);
            }
        }
    }
}
