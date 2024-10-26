using DCFrameWork.DefenseEquipment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTurretManager : DEEntityManager_SB<DefenseEquipmentData_B>
{
    [SerializeField] float _minRange = 1;
    [SerializeField] Vector3 _boxCastSize = new Vector3(1, 1, 1);
    [SerializeField] LayerMask _ground;
    protected override void Think() //UpDate ‚Æ“¯‹`
    {
        Vector3 pos = SummonPosition();
        int count = 0;
        while (!Check(pos) || count <= 10)
        {
            Debug.Log(pos);
            pos = SummonPosition();
            count++;

        }
        count = 0;
        Summon(pos);
    }
    bool Check(Vector3 pos)
    {
        Physics.BoxCast(pos, _boxCastSize, Vector3.down, out RaycastHit hit);
        return hit.collider.gameObject.layer == _ground;
    }

    Vector3 SummonPosition()
    {
        float r = Random.Range(_minRange, _range);
        float degree = Random.Range(0, 360);
        float radian = degree * Mathf.Deg2Rad;
        float randomPosX = r * Mathf.Cos(radian);
        float randomPosZ = r * Mathf.Sin(radian);
        return transform.position + new Vector3(randomPosX, 0, randomPosZ);
    }
    protected override void LoadSpecificData(DefenseEquipmentData_B data)
    {
        throw new System.NotImplementedException();
    }

    protected override void Pause()
    {
        throw new System.NotImplementedException();
    }

    protected override void Resume()
    {
        throw new System.NotImplementedException();
    }

}
