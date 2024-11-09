using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] Transform _spawnPos;
    [SerializeField] Transform _targetPos;
    [SerializeField] GameObject _floorPrefab;
    [SerializeField] Vector3 _floorCenter;
    [SerializeField] float _gridSize = 5f;
    [SerializeField] List<ObstaclePrefabs> _obstaclePrefabList;    
    //[SerializeField] GameObject _clickPointPrefab;//Debug;
    [SerializeField] NavMeshSurface _navMeshSurface;
    [SerializeField] GameObject _wallsParent;
    [Serializable]
    public struct ObstaclePrefabs
    {
        public string Name;
        public GameObject PutObstaclePrefab;
        public GameObject VisualGuide;
    }
    GameObject _setPrefab;
    GameObject _tentativePrefab;
    private Camera _mainCamera;
    private Vector3 _currentPosition = Vector3.zero;
    //private float _prefabHeight;
    bool _canSet = false;
    int[,] _map;
    int _sizeX;
    int _sizeZ;
    int _noWall;
    int _startX = 1;
    int _startZ = 1;
    //計算に関するコメントアウトのメモが噓をついている可能性があります。鵜吞みにしないように。発見したら教えてください。
    void Start()
    {
        //_navMeshSurface.BuildNavMesh();
        _mainCamera = Camera.main;
        //_prefabHeight = _setPrefab.GetComponent<BoxCollider>().size.y;
        //何マスx何マスかを調べる
        _sizeX = (int)(_floorPrefab.transform.localScale.x / _gridSize);
        _sizeZ = (int)(_floorPrefab.transform.localScale.z / _gridSize);
        _map = new int[_sizeX, _sizeZ];
        //デフォルトで配置するオブジェクトをセットしてる。後から消すかも？
        _setPrefab = _obstaclePrefabList[0].PutObstaclePrefab;
        _tentativePrefab = _obstaclePrefabList[0].VisualGuide;
        LoadStage();
        //ステージのグリッド座標に壁があるかしらべて2次元配列に格納
        void LoadStage()
        {
            //(localScale/2-trans.pos + _gridsize/2) + _gridsize * i(0<=i<=sizeXZ) = グリッドの各マスの中心座標のx,z
            for (int i = 0; i < _sizeZ; i++)
            {
                for (int j = 0; j < _sizeX; j++)
                {
                    Vector3 vector3 = new Vector3((_floorCenter.x - _floorPrefab.transform.localScale.x / 2 + _gridSize / 2) + _gridSize * j,
                                                  7.5f,
                                                 (_floorCenter.z - _floorPrefab.transform.localScale.z / 2 + _gridSize / 2) + _gridSize * i);
                    if (Physics.Raycast(vector3, Vector3.down, out RaycastHit hit, 5, LayerMask.GetMask("Ground")))
                    {
                        _map[j, i] = 1;
                    }
                    else
                    {
                        _map[j, i] = 0;
                        //壁のない場所を数える
                        _noWall++;
                    }
                    //Debug.Log($"{vector3}:{_map[j, i]}:({j},{i})={(hit.collider != null ? hit.collider.gameObject.name : null)}");
                    //Debug.DrawRay(vector3, Vector3.down * 5, Color.blue, 10);
                }
            }
        }
    }
    void Update()
    {
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        var raycastHitList = Physics.RaycastAll(ray, float.PositiveInfinity).ToList();
        var beforeCurrentPos = _currentPosition;
        if (raycastHitList.Any())
        {
            //置く場所を視覚的にサポートするオブジェクトとはレイキャストが当たってないことにする
            raycastHitList.Remove(raycastHitList.Where(x => x.collider.gameObject == _tentativePrefab).FirstOrDefault());
            var hit = raycastHitList.OrderByDescending(x => x.collider.gameObject.transform.position.y).FirstOrDefault();
            _currentPosition = hit.point;
            Debug.Log($"{hit.collider.gameObject.name}:{hit.collider.gameObject.layer}:{ LayerMask.NameToLayer("Ground")}");
            //_clickPointPrefab.transform.position = _currentPosition;
            //グリッドの計算式
            _currentPosition.y = (int)((_currentPosition.y + hit.normal.y / 2) / 5) * 5 + 2.5f;
            _currentPosition.x = (int)((_currentPosition.x + hit.normal.x / 2) / 5) * 5 + 2.5f * Mathf.Sign(_currentPosition.x);
            _currentPosition.z = (int)((_currentPosition.z + hit.normal.z / 2) / 5) * 5 + 2.5f * Mathf.Sign(_currentPosition.z);
            //マウスと重なっているグリッドの中心座標に視覚的にサポートするオブジェクトをセット
            _tentativePrefab.transform.position = _currentPosition;
            //Debug.DrawRay(_currentPosition, Vector3.down, Color.green, 1f);
            //ステージの範囲外に出てたら見えなくする
            if (_tentativePrefab.transform.position.y > 8f || !Physics.Raycast(_currentPosition, Vector3.down, 5f, LayerMask.GetMask("Ground")))
            {
                _canSet = false;
                _tentativePrefab.SetActive(false);
            }
            else
            {
                _canSet = true;
                _tentativePrefab.SetActive(true);
            }
            //置けるかどうかの判定
            if (CheckStage(_currentPosition) && _currentPosition != _spawnPos.position && _currentPosition != _targetPos.position && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _tentativePrefab.GetComponent<MeshRenderer>().material.color = Color.white;
            }
            else
            {
                _tentativePrefab.GetComponent<MeshRenderer>().material.color = Color.red;
                _canSet = false;
            }
        }
        //オブジェクトを置く処理
        if (Input.GetMouseButtonDown(0))
        {
            if (_canSet)
            {
                SetObject(_currentPosition);
            }
        }
    }
    public bool CheckStage(Vector3 currentPosition)
    {
        //i = (グリッドの各マスの中心座標のx,z -(_floorPrefab.transform.position.z - _floorPrefab.transform.localScale.z / 2 + _gridSize / 2))/_gridSize
        int currentX;
        int currentZ;
        //(-42.5-2.5 + 47.5 - 2.5)/5
        currentX = (int)((currentPosition.x - _floorCenter.x + _floorPrefab.transform.localScale.x / 2 - _gridSize / 2) / _gridSize);
        currentZ = (int)((currentPosition.z - _floorCenter.z + _floorPrefab.transform.localScale.z / 2 - _gridSize / 2) / _gridSize);
        //Debug.Log($"{currentX},{currentZ}");
        if (currentX < 0 || currentZ < 0 || currentX >= _sizeX || currentZ >= _sizeZ) { return false; }
        int[,] subMap = new int[_sizeX, _sizeZ];
        for (int i = 0; i < _sizeZ; i++)
        {
            string s = "";
            for (int j = 0; j < _sizeX; j++)
            {
                subMap[j, i] = _map[j, i];
                s += subMap[j, i];
            }
            //Debug.Log(s);
        }
        if (subMap[currentX, currentZ] == 1)
        {
            return true;
        }
        subMap[currentX, currentZ] = 1;
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(_startX);
        queue.Enqueue(_startZ);
        int count = 0;
        while (queue.Count > 0)
        {
            count++;
            int x = queue.Dequeue();
            int z = queue.Dequeue();
            //Debug.Log($"{x},{z}");
            if (x + 1 < _sizeX && subMap[x + 1, z] == 0)
            {
                subMap[x + 1, z] = 1;
                queue.Enqueue(x + 1);
                queue.Enqueue(z);
            }
            if (x - 1 >= 0 && subMap[x - 1, z] == 0)
            {
                subMap[x - 1, z] = 1;
                queue.Enqueue(x - 1);
                queue.Enqueue(z);
            }
            if (z + 1 < _sizeZ && subMap[x, z + 1] == 0)
            {
                subMap[x, z + 1] = 1;
                queue.Enqueue(x);
                queue.Enqueue(z + 1);
            }
            if (z - 1 >= 0 && subMap[x, z - 1] == 0)
            {
                subMap[x, z - 1] = 1;
                queue.Enqueue(x);
                queue.Enqueue(z - 1);
            }
        }
        //Debug.Log($"{count},{_noWall}");
        return count == _noWall;
    }
    public void SetObject(Vector3 currentPosition)
    {
        int currentX;
        int currentZ;
        //(-42.5-2.5 + 47.5 - 2.5)/5
        //オブジェクトを置こうとしている座標がグリッド座標のどこかを調べる
        currentX = (int)((currentPosition.x - _floorCenter.x + _floorPrefab.transform.localScale.x / 2 - _gridSize / 2) / _gridSize);
        currentZ = (int)((currentPosition.z - _floorCenter.z + _floorPrefab.transform.localScale.z / 2 - _gridSize / 2) / _gridSize);
        //マップ情報の更新
        if (_map[currentX, currentZ] == 0)
        {
            _noWall--;
        }
        _map[currentX, currentZ] = 1;
        //生成
        var obj = Instantiate(_setPrefab, _currentPosition, Quaternion.identity);
        obj.transform.SetParent(_wallsParent.transform);
    }
    //設置するオブジェクトの変更
    public void ChangeObstaclePrefab(string name)
    {
        ObstaclePrefabs p = _obstaclePrefabList.Find(x => x.Name == name);
        _setPrefab = p.PutObstaclePrefab;
        _tentativePrefab = p.VisualGuide;
    }
}
