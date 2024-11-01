using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] Transform _tagetPos;
    [SerializeField] GameObject _airPrefab;
    [SerializeField] GameObject _floorPrefab;
    [SerializeField] float _gridSize = 5f;
    int[,] _map;
    int _sizeX;
    int _sizeZ;
    int _noWall;
    int _startX = 1;
    int _startZ = 1;
    void Start()
    {
        _sizeX = (int)(_floorPrefab.transform.localScale.x / _gridSize);
        _sizeZ = (int)(_floorPrefab.transform.localScale.z / _gridSize);
        _map = new int[_sizeX, _sizeZ];
        LoadStage();
        void LoadStage()
        {
            //(localScale/2-trans.pos + _gridsize/2) + _gridsize * i(0<=i<=sizeXZ) = グリッドの各マスの中心座標のx,z
            for (int i = 0; i < _sizeZ; i++)
            {
                for (int j = 0; j < _sizeX; j++)
                {
                    Vector3 vector3 = new Vector3((_floorPrefab.transform.position.x - _floorPrefab.transform.localScale.x / 2 + _gridSize / 2) + _gridSize * j,
                                                  7.5f,
                                                 (_floorPrefab.transform.position.z - _floorPrefab.transform.localScale.z / 2 + _gridSize / 2) + _gridSize * i);
                    if (Physics.Raycast(vector3, Vector3.down, out RaycastHit hit, 5, LayerMask.GetMask("Ground")))
                    {
                        _map[j, i] = 1;
                    }
                    else
                    {
                        _map[j, i] = 0;
                        _noWall++;
                    }
                    Debug.Log($"{vector3}:{_map[j, i]}:({j},{i})={(hit.collider != null ? hit.collider.gameObject.name : null)}");
                    Debug.DrawRay(vector3, Vector3.down * 5, Color.blue, 10);
                }
            }
            Debug.Log($"_nowall = {_noWall}");
            //for(int i = 0; i < _sizeZ; ++i)
            //{
            //    string s = "";
            //    for(int j  = 0; j < _sizeX; ++j)
            //    {
            //        s += _subMap[j, i];
            //    }
            //    Debug.Log(s);
            //}
            //_startX = (int)((_tagetPos.position.x - _floorPrefab.transform.position.x + _floorPrefab.transform.localScale.x / 2 - _gridSize / 2) / _gridSize);
            //_startZ = (int)((_tagetPos.position.z - _floorPrefab.transform.position.z + _floorPrefab.transform.localScale.z / 2 - _gridSize / 2) / _gridSize);
        }
        //Instantiate(_airPrefab, _tagetPos.position, Quaternion.identity);
        //FillAir();
    }
    //H
    // Update is called once per frame
    void Update()
    {

    }
    public void CheckStage(Vector3 currentPosition)
    {
        //i = (グリッドの各マスの中心座標のx,z -(_floorPrefab.transform.position.z - _floorPrefab.transform.localScale.z / 2 + _gridSize / 2))/_gridSize
        int currentX;
        int currentZ;
        //(-42.5-2.5 + 47.5 - 2.5)/5
        currentX = (int)((currentPosition.x - _floorPrefab.transform.position.x + _floorPrefab.transform.localScale.x / 2 - _gridSize / 2) / _gridSize);
        currentZ = (int)((currentPosition.z - _floorPrefab.transform.position.z + _floorPrefab.transform.localScale.z / 2 - _gridSize / 2) / _gridSize);
        //Debug.Log($"{currentX},{currentZ}");
        int[,] subMap = new int[_sizeX,_sizeZ];
        for (int i = 0; i < _sizeZ; i++)
        {
            string s = "";
            for (int j = 0; j < _sizeX; j++)
            {
                subMap[j,i] = _map[j,i];
                s += subMap[j, i];
            }
            Debug.Log(s);
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
            Debug.Log($"{x},{z}");
            if (subMap[x + 1, z] == 0)
            {
                subMap[x + 1, z] = 1;
                queue.Enqueue(x + 1);
                queue.Enqueue(z);
            }
            if (subMap[x - 1, z] == 0)
            {
                subMap[x - 1, z] = 1;
                queue.Enqueue(x - 1);
                queue.Enqueue(z);
            }
            if (subMap[x, z + 1] == 0)
            {
                subMap[x, z + 1] = 1;
                queue.Enqueue(x);
                queue.Enqueue(z + 1);
            }
            if (subMap[x, z - 1] == 0)
            {
                subMap[x, z - 1] = 1;
                queue.Enqueue(x);
                queue.Enqueue(z - 1);
            }
        }
        Debug.Log($"{count},{_noWall}");
    }
    void FillAir()
    {
        float time = Time.time;
        Queue<Vector3> airPos = new Queue<Vector3>();
        airPos.Enqueue(_tagetPos.position);
        while (airPos.Count > 0)
        {
            Debug.Log("実行中");
            Vector3 pos = airPos.Dequeue();
            if (!Physics.Raycast(pos, Vector3.right, 1))
            {
                airPos.Enqueue(pos + Vector3.right);
                Instantiate(_airPrefab, pos + Vector3.right, Quaternion.identity);
            }
            if (!Physics.Raycast(pos, Vector3.left, 1))
            {
                airPos.Enqueue(pos + Vector3.left);
                Instantiate(_airPrefab, pos + Vector3.left, Quaternion.identity);
            }
            if (!Physics.Raycast(pos, Vector3.forward, 1))
            {
                airPos.Enqueue(pos + Vector3.forward);
                Instantiate(_airPrefab, pos + Vector3.forward, Quaternion.identity);
            }
            if (!Physics.Raycast(pos, Vector3.back, 1))
            {
                airPos.Enqueue(pos + Vector3.back);
                Instantiate(_airPrefab, pos + Vector3.back, Quaternion.identity);
            }
        }
    }
}
