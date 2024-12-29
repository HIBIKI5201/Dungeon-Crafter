using DCFrameWork.DefenseEquipment;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DCFrameWork.DefenseEquipment
{
    public abstract class DEEntityManager_SB<Data> : DefenseEquipmentManager_B<Data> where Data : DefenseEquipmentData_B
    {
        [SerializeField]
        float _minRange = 1;
        [SerializeField]
        protected Vector3 _boxCastSize = new(1, 10, 1);
        [SerializeField]
        protected LayerMask _groundLayer;
        [SerializeField]
        protected GameObject _entityPrefab;

        protected List<GameObject> _entityList = new List<GameObject>();
        protected virtual async void Summon(Vector3 pos, int count)
        {
            if (_entityList.Count < count)
            {
                if (_entityPrefab is null) return;
                var entity = InstantiateAsync(_entityPrefab, transform, new Vector3(pos.x, 0, pos.z), Quaternion.identity);
                GameObject obj = (await entity)[0];
                _entityList.Add(obj);
            }
        }
        virtual protected bool Check(Vector3 pos)
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
            float r = Random.Range(_minRange, Range * 5);
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