#define DEBUGGING_TRAP_TURR_MAN
#undef DEBUGGING_TRAP_TURR_MAN
using DCFrameWork.DefenseEquipment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTurretManager : DEEntityManager_SB<DefenseEquipmentData_B>
{
    [SerializeField] float _minRange = 1;
    [SerializeField] Vector3 _boxCastSize = new Vector3(1, 1, 1);
    [SerializeField] LayerMask _groundLayer;
    Vector3 _position;
    protected override void Think() //UpDate ‚Æ“¯‹`
    {
        _position = SummonPosition();
        int count = 0;
        while (!Check(_position) && count <= 10)
        {
            _position = SummonPosition();
            count++;
        }
        Debug.Log(_position);
        Summon(_position);
    }
    bool Check(Vector3 pos)
    {
        Physics.BoxCast(pos, _boxCastSize, Vector3.down, out RaycastHit hit);
        if (hit.collider == null)
        {
#if DEBUGGING_TRAP_TURR_MAN
            Debug.Log("collider is null");
#endif
            return false;
        }
        if (hit.collider.gameObject == null)
        {
#if DEBUGGING_TRAP_TURR_MAN
            Debug.Log("gameObject is null");
#endif
        }
        Debug.Log(hit.collider.gameObject.layer == _groundLayer);
        return hit.collider.gameObject.layer == _groundLayer;
    }

    private void OnDrawGizmos()
    {
#if DEBUGGIN_TRUP_TURR_MAN
        Debug.Log("ƒMƒYƒ‚‚ð•\Ž¦‚µ‚Ä‚¢‚Ü‚·");
#endif
        Gizmos.color = Check(_position) ? Color.red : Color.green;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, _boxCastSize);
        Gizmos.DrawCube(SummonPosition(), new Vector3(1, 1, 1));
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
