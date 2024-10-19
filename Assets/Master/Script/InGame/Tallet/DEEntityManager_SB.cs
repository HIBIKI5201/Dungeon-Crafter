using DCFrameWork.DefenseEquipment;
using System.Collections.Generic;
using UnityEngine;

public abstract class DEEntityManager_SB : DefenseEquipmentData_B
{
    [SerializeField]
    GameObject _entityPrefab;

    List<GameObject> _entityList = new List<GameObject>();

    [SerializeField]
    Transform _summonPos;

    protected void Summon()
    {
        if (_entityPrefab is null) return;
        _entityList.Add(Instantiate(_entityPrefab, _summonPos.position, Quaternion.identity));
    }
}
