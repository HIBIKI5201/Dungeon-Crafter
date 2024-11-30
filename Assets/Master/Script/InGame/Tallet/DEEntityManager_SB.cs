using DCFrameWork.DefenseEquipment;
using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public abstract class DEEntityManager_SB<Data> : DefenseEquipmentManager_B<Data> where Data : DefenseEquipmentData_B
    {
        [SerializeField]
        float _minRange = 1;
        [SerializeField]
        Vector3 _boxCastSize = new(1, 10, 1);
        [SerializeField]
        LayerMask _groundLayer;
        [SerializeField]
        GameObject _entityPrefab;

        List<GameObject> _entityList = new List<GameObject>();
        protected virtual void Summon(Vector3 pos, int count,Transform entityStorage)
        {
            if (_entityList.Count < count)
            {
                if (_entityPrefab is null) return;
                var entity = Instantiate(_entityPrefab, new Vector3(pos.x, 0, pos.z), Quaternion.identity, entityStorage);
                _entityList.Add(entity);
            }
        }
        protected bool Check(Vector3 pos)
        {
            Physics.BoxCast(pos + new Vector3(0, 8, 0), _boxCastSize / 2, Vector3.down, out RaycastHit hit, Quaternion.identity, 18f);

            if (hit.collider != null)
            {
                return hit.collider.gameObject.layer == Mathf.Log(_groundLayer.value, 2);
            }
            return false;
        }

        protected Vector3 SummonPosition()
        {
            float r = Random.Range(_minRange, DefenseEquipmentData.Range);
            float degree = Random.Range(0, 360);
            float radian = degree * Mathf.Deg2Rad;
            float randomPosX = r * Mathf.Cos(radian);
            float randomPosZ = r * Mathf.Sin(radian);
            return transform.position + new Vector3(randomPosX, 0, randomPosZ);
        }
        protected override void Start_SB()
        {
            Start_S();
        }
        protected virtual void Start_S() { }
    }
}