using DCFrameWork.DefenseEquipment;
using System;
using UnityEngine;

namespace DCFrameWork
{
    [Serializable, CreateAssetMenu(menuName = "GameData/DefenseData/ShortRange_Data", fileName = "DefebseEquipmentData")]

    public class ShortRangeData : DefenseEquipmentData_B
    {
        public float ExplosionRadius { get => _explosionRadius; }
        [SerializeField, Header("爆発の範囲")]
        private float _explosionRadius = 1;
    }
}
