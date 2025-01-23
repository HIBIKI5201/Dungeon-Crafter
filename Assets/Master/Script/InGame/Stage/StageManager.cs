using DCFrameWork;
using DCFrameWork.DefenseEquipment;
using DCFrameWork.Enemy;
using DCFrameWork.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] Vector2 _mapSize = new Vector2(10, 10);
    [SerializeField] float _gridSize = 5f;
    [SerializeField] List<ObstaclePrefabs> _obstaclePrefabList;
    [SerializeField] GameObject _defaultVisualGuide;
    [SerializeField] GameObject _obstacleWallPrefab;
    [SerializeField] InGameUIManager _inGameUIManager;
    [SerializeField] PlayerManager _playerManager;
    //[SerializeField] GameObject _clickPointPrefab;//Debug;
    [Serializable]
    public struct ObstaclePrefabs
    {
        public string Name;
        public GameObject PutObstaclePrefab;
        [Tooltip("特注の視覚サポートオブジェクトがあればいれてください")] public GameObject VisualGuide;
        public bool IsPutTogetherWithWall;
    }
    public event Action<ITurret, bool> OnActivateTurretSelectedUI;//タレットがクリックされたときに呼ぶ処理
    Vector3[] _spawnPos;
    Vector3 _targetPos;
    GameObject _setPrefab;
    GameObject _tentativePrefab;
    GameObject _wallsParent;
    private Camera _mainCamera;
    private Vector3 _currentPosition = Vector3.zero;
    private GameObject _selectedTurret;
    //private float _prefabHeight;
    bool _canSet = false;
    bool _isPutTogetherWithWall;
    int[,] _map;
    int _sizeX;
    int _sizeZ;
    int _noWall;
    int _startX = 1;
    int _startZ = 1;
    bool _isMouseOnUI = false;
    //計算に関するコメントアウトのメモが噓をついている可能性があります。鵜吞みにしないように。発見したら教えてください。
    void Start()
    {
        _inGameUIManager.OnMouseOnUI += GetMouseOnUI;
        _inGameUIManager.OnTurretDelete += RemoveObject;
        var enemyGenerator = GetComponentInChildren<EnemyGenerator>();
        _spawnPos = new Vector3[enemyGenerator.SpawnPos.Length];
        _targetPos = enemyGenerator.TargetPos.position;
        //スポーン地点と目標地点に障害物を置けないようにするための調整
        for (int i = 0; i < enemyGenerator.SpawnPos.Length; i++)
        {
            _spawnPos[i] = enemyGenerator.SpawnPos[i].position;
            _spawnPos[i] = new Vector3(_spawnPos[i].x, 2.5f, _spawnPos[i].z);
        }
        _targetPos = new Vector3(_targetPos.x, 2.5f, _targetPos.z);
        _wallsParent = new GameObject();
        _wallsParent.transform.SetParent(transform);
        _wallsParent.name = "Obstacle Parent";
        _mainCamera = Camera.main;
        //_floorCenter = new Vector3(_floorPrefab.transform.position.x + _floorPrefab.transform.localScale.x / 2, _floorPrefab.transform.position.y, _floorPrefab.transform.position.z - _floorPrefab.transform.localScale.z / 2);
        //何マスx何マスかを調べる
        //_sizeX = (int)(_floorPrefab.transform.localScale.x / _gridSize);
        //_sizeZ = (int)(_floorPrefab.transform.localScale.z / _gridSize);
        _sizeX = (int)(_mapSize.x);
        _sizeZ = (int)(_mapSize.y);
        _map = new int[_sizeX, _sizeZ];
        _startX = (int)((_targetPos.x + _mapSize.x * _gridSize / 2 - _gridSize / 2) / _gridSize);
        _startZ = (int)((_targetPos.z + _mapSize.y * _gridSize / 2 - _gridSize / 2) / _gridSize);
        if (_obstaclePrefabList[0].VisualGuide == null)
        {
            _tentativePrefab = _defaultVisualGuide;
        }
        else
        {
            _tentativePrefab = _obstaclePrefabList[0].VisualGuide;
        }
        LoadStage();
        //ステージのグリッド座標に壁があるかしらべて2次元配列に格納
        void LoadStage()
        {
            //string s = "";
            //(localScale/2-trans.pos + _gridsize/2) + _gridsize * i(0<=i<=sizeXZ) = グリッドの各マスの中心座標のx,z
            for (int i = 0; i < _sizeZ; i++)
            {
                //s += "/";
                for (int j = 0; j < _sizeX; j++)
                {
                    Vector3 vector3 = new Vector3((-_mapSize.x * _gridSize / 2 + _gridSize / 2) + _gridSize * j,
                                                  17.5f,
                                                 (-_mapSize.y * _gridSize / 2 + _gridSize / 2) + _gridSize * i);
                    if (Physics.Raycast(vector3, Vector3.down, 20, LayerMask.GetMask("Buildings")))
                    {
                        _map[j, i] = 1;
                    }
                    else
                    {
                        _noWall++;
                    }
                    //s += _map[j, i];
                }
            }
            //Debug.Log(s);
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SetTurretCancel();
        }
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new();
        var raycastHitList = Physics.RaycastAll(ray, float.PositiveInfinity).Where(x => !x.collider.isTrigger).ToList();
        if (raycastHitList.Any())
        {
            //置く場所を視覚的にサポートするオブジェクトとはレイキャストが当たってないことにする
            raycastHitList.Remove(raycastHitList.Where(x => x.collider.gameObject == _tentativePrefab).FirstOrDefault());
            hit = raycastHitList.OrderBy(x => Vector3.Distance(x.point, Camera.main.transform.position)).FirstOrDefault();
            _currentPosition = hit.point;
            //Debug.Log($"{hit.point}:{hit.collider.gameObject.name}:{hit.collider.gameObject.layer}:{LayerMask.NameToLayer("Ground")}");
            float currentX = _currentPosition.x + hit.normal.x;
            float currentZ = _currentPosition.z + hit.normal.z;
            int signX = Math.Sign(currentX);
            int signZ = Math.Sign(currentZ);
            float halfGridSize = _gridSize / 2;
            //グリッドの計算式
            _currentPosition.y = (int)((_currentPosition.y + hit.normal.y) / _gridSize) * _gridSize + halfGridSize;
            if (_mapSize.x % 2 == 0)
                _currentPosition.x = (int)(currentX / _gridSize) * _gridSize + halfGridSize * signX;
            else
                _currentPosition.x = (int)(currentX / halfGridSize + signX) / 2 * _gridSize;
            if (_mapSize.y % 2 == 0)
                _currentPosition.z = (int)(currentZ / _gridSize) * _gridSize + halfGridSize * signZ;
            else
                _currentPosition.z = (int)(currentZ / halfGridSize + signZ) / 2 * _gridSize;
            //マウスと重なっているグリッドの中心座標に視覚的にサポートするオブジェクトをセット
            _tentativePrefab.transform.position = _currentPosition;

            //Debug.Log(_currentPosition);
            //Debug.DrawRay(_currentPosition, Vector3.down, Color.green, 1f);
            //ステージの範囲外に出てたら見えなくする
            if (_setPrefab == null || _tentativePrefab.transform.position.y > 8f || !Physics.Raycast(_currentPosition, Vector3.down, 5f))
            {
                _canSet = false;
                _tentativePrefab.SetActive(false);
            }
            else
            {
                _tentativePrefab.SetActive(true);
                //置けるかどうかの判定
                if (CheckConnected(_currentPosition) && !_spawnPos.Contains(_currentPosition) && _currentPosition != _targetPos && _currentPosition != _targetPos + new Vector3(0, _gridSize, 0))
                {
                    _tentativePrefab.GetComponent<MeshRenderer>().material.color = Color.white;
                    _canSet = true;
                }
                else
                {
                    _tentativePrefab.GetComponent<MeshRenderer>().material.color = Color.red;
                    _canSet = false;
                }
            }
        }
        else
        {
            _canSet = false;
            _tentativePrefab.SetActive(false);
        }
        //オブジェクトを置く処理
        if (Input.GetMouseButtonDown(0))
        {
            if (_canSet && !_isMouseOnUI)
            {
                SetObject(_currentPosition);
            }
            else if (hit.collider != null)
            {
                TurretSelect(hit.collider.gameObject);
                //RemoveObject();
            }
        }
    }
    void GetMouseOnUI(bool isMouseOnUI)
    {
        _isMouseOnUI = isMouseOnUI;
    }
    void TurretSelect(GameObject turret)
    {
        if (turret.TryGetComponent<ITurret>(out ITurret t))
        {
            //Debug.Log("タレットがクリックされた");
            bool canRemove = true;
            canRemove = CheckCanRemove();
            OnActivateTurretSelectedUI?.Invoke(t, canRemove);
            _selectedTurret = turret;
            bool CheckCanRemove()
            {
                int currentX;
                int currentZ;
                currentX = (int)((t.transform.position.x + _mapSize.x * _gridSize / 2 - _gridSize / 2) / _gridSize);
                currentZ = (int)((t.transform.position.z + _mapSize.y * _gridSize / 2 - _gridSize / 2) / _gridSize);
                //座標に1を足したり引いたりした時に、配列の範囲外に出た時と、計算した座標がの値が1(壁がある)のときtrue
                if ((currentX <= 0 || _map[currentX - 1, currentZ] == 1) &&
                    (currentZ <= 0 || _map[currentX, currentZ - 1] == 1) &&
                    (currentX >= _sizeX - 1 || _map[currentX + 1, currentZ] == 1) &&
                    (currentZ >= _sizeZ - 1 || _map[currentX, currentZ + 1] == 1))
                {
                    return false;
                }//4方向に壁がある時、そのタレットは削除できないようにするためfalseを返す
                else
                {
                    return true;
                }//1つの方向でも壁がなかったら消せる
            }
        }
    }
    private bool CheckConnected(Vector3 currentPosition)
    {
        //i = (グリッドの各マスの中心座標のx,z -(_floorPrefab.transform.position.z - _floorPrefab.transform.localScale.z / 2 + _gridSize / 2))/_gridSize
        int currentX;
        int currentZ;
        currentX = (int)((currentPosition.x + _mapSize.x * _gridSize / 2 - _gridSize / 2) / _gridSize);
        currentZ = (int)((currentPosition.z + _mapSize.y * _gridSize / 2 - _gridSize / 2) / _gridSize);
        //Debug.Log($"{currentX},{currentZ}");
        if (currentX < 0 || currentZ < 0 || currentX >= _sizeX || currentZ >= _sizeZ) { return false; }
        int[,] subMap = new int[_sizeX, _sizeZ];
        for (int i = 0; i < _sizeZ; i++)
        {
            for (int j = 0; j < _sizeX; j++)
            {
                subMap[j, i] = _map[j, i];
            }
        }
        //Debug.Log($"({currentX},{currentZ}):{subMap[currentX, currentZ]}");
        if (subMap[currentX, currentZ] == 1)
        {
            return true;
        }
        else if (subMap[currentX, currentZ] == 2)
        {
            return false;
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
        currentX = (int)((currentPosition.x + _mapSize.x * _gridSize / 2 - _gridSize / 2) / _gridSize);
        currentZ = (int)((currentPosition.z + _mapSize.y * _gridSize / 2 - _gridSize / 2) / _gridSize);
        //マップ情報の更新
        if (_map[currentX, currentZ] == 0)
        {
            _noWall--;
        }
        _map[currentX, currentZ] = 1;
        //生成
        if (currentPosition.y == 2.5f)
        {
            var wallObj = Instantiate(_obstacleWallPrefab, _currentPosition, Quaternion.identity);
            wallObj.transform.SetParent(_wallsParent.transform);
            wallObj.isStatic = true;
            _currentPosition.y = 7.5f;
        }
        var obj = Instantiate(_setPrefab, _currentPosition, Quaternion.Euler(new Vector3(0,180,0)));
        obj.transform.SetParent(_wallsParent.transform);
        obj.isStatic = true;
        _setPrefab = null;
    }
    public void RemoveObstacleObject(GameObject gameObject)
    {
        //消す方法が決まっていないので一旦引数に消すオブジェクトを指定するようにする
        int currentX;
        int currentZ;
        //消そうとしているオブジェクトの座標がグリッド座標のどこかを調べて範囲外か何もないなら処理を終了
        currentX = (int)((gameObject.transform.position.x + _mapSize.x * _gridSize / 2 - _gridSize / 2) / _gridSize);
        currentZ = (int)((gameObject.transform.position.z + _mapSize.y * _gridSize / 2 - _gridSize / 2) / _gridSize);
        if (currentX < 0 || currentZ < 0 || currentX >= _sizeX || currentZ >= _sizeZ || _map[currentX, currentZ] == 0) { return; }
        //障害物を置けない高さから消すオブジェクトを全取得
        foreach (var obj in Physics.RaycastAll(gameObject.transform.position + Vector3.up * 8, Vector3.down, 20, LayerMask.GetMask("Buildings")).Where(hit => !hit.collider.isTrigger))
        {
            Destroy(obj.collider.gameObject);
        }
        //マップ情報を更新
        _map[currentX, currentZ] = 0;
        _noWall++;
    }
    //設置するオブジェクトの変更。これは削除することになるだろうから変更はあまり気にしないでください。参照もないし
    public void ChangeObstaclePrefab(string name)
    {
        ObstaclePrefabs p = _obstaclePrefabList.Find(x => x.Name == name);
        _setPrefab = p.PutObstaclePrefab;
        _tentativePrefab = p.VisualGuide;
        if (_tentativePrefab != null)
        {
            _tentativePrefab = _defaultVisualGuide;
        }
    }
    /// <summary>
    /// インベントリから選ばれたタレットのGameObjectを渡してください
    /// </summary>
    /// <param name="turret"></param>
    public void SetTurret(GameObject turret)
    {
        SetTurretCancel();
        _setPrefab = turret;
    }
    void SetTurretCancel()
    {
        if (_setPrefab != null)
        {
            _setPrefab.TryGetComponent<ITurret>(out ITurret t);
            _playerManager.SetDefenseObject(t.Data.Kind);
            _setPrefab = null;
        }
    }
    /// <summary>
    /// 引数にワールド座標を入れると、
    /// 対応したステージ上の座標に壁などが設置されているかを調べて、
    /// なければtrueあればfalseを返します
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <returns></returns>
    public bool CheckObjectPlaced(Vector3 currentPosition)
    {
        int currentX;
        int currentZ;
        currentX = (int)((currentPosition.x + _mapSize.x * _gridSize / 2 - _gridSize / 2) / _gridSize);
        currentZ = (int)((currentPosition.z + _mapSize.y * _gridSize / 2 - _gridSize / 2) / _gridSize);
        if (currentX < 0 || currentZ < 0 || currentX >= _sizeX || currentZ >= _sizeZ)
        {
            return false;
        }
        else
        {
            return _map[currentX, currentZ] == 0 ? true : false;
        }
    }
    public Vector3 GetGroundScale()
    {
        return new Vector3(_mapSize.x, 0, _mapSize.y) * _gridSize / 2;
    }
    /// <summary>
    /// 指定座標のオブジェクト全部消す
    /// </summary>
    /// <param name="currentPosition"></param>
    public void RemoveObject()
    {
        if (_selectedTurret == null)
        {
            //タレットが選択された状態でしかこのメソッドは呼ばれないはずなので
            Debug.LogWarning("削除するタレットが選択されていません。不具合の可能性があります");
            return;
        }
        Vector3 currentPosition = _selectedTurret.transform.position;
        _selectedTurret.TryGetComponent<ITurret>(out ITurret t);
        _playerManager.SetDefenseObject(t.Data.Kind);
        int currentX;
        int currentZ;
        //オブジェクトを消そうとしている座標がグリッド座標のどこかを調べる
        currentX = (int)((currentPosition.x + _mapSize.x * _gridSize / 2 - _gridSize / 2) / _gridSize);
        currentZ = (int)((currentPosition.z + _mapSize.y * _gridSize / 2 - _gridSize / 2) / _gridSize);

        //オブジェクト消す処理どうする？(一旦指定座標のオブジェクト全部消す方針)
        currentPosition.y = 20;
        RaycastHit[] hits = Physics.RaycastAll(currentPosition, Vector3.down, 20, LayerMask.GetMask("Buildings"));
        foreach (RaycastHit hit in hits)
        {
            if (!hit.collider.isTrigger)
            {
                Destroy(hit.collider.gameObject);
            }
        }
        //ステージ情報の更新
        _map[currentX, currentZ] = 0;
        _noWall++;
        _selectedTurret = null;
    }
}
