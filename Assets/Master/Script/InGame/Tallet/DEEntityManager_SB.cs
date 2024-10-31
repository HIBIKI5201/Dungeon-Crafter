using DCFrameWork.DefenseEquipment;
using System.Collections.Generic;
using UnityEngine;

public abstract class DEEntityManager_SB<Data> : DefenseEquipmentManager_B<Data> where Data : DefenseEquipmentData_B
{
    [SerializeField]
    GameObject _entityPrefab;

    List<GameObject> _entityList = new List<GameObject>();
    protected void Summon(Vector3 pos)
    {
        if (_entityPrefab is null) return;
        _entityList.Add(Instantiate(_entityPrefab, pos, Quaternion.identity));
    }
}
