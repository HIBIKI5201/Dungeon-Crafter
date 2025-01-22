using DCFrameWork.DefenseEquipment;
using System;
using UnityEngine;

namespace DCFrameWork
{
    [Serializable, CreateAssetMenu(menuName = "GameData/DefenseData/LongRange_Data", fileName = "DefebseEquipmentData")]

    public class LongRangeShooterData : DefenseEquipmentData_B
    {
        public int PierceCount { get => _pierceCount; }
        [SerializeField,Header("貫通可能数")]
        private int _pierceCount = 1;
    }
}
