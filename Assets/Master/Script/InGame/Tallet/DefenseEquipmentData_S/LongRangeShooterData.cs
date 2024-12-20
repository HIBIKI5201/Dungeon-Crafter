using DCFrameWork.DefenseEquipment;
using System;
using UnityEngine;

namespace DCFrameWork
{
    [Serializable, CreateAssetMenu(menuName = "GameData/DefenseData/LongRange_Data", fileName = "DefebseEquipmentData")]

    public class LongRangeShooterData : DefenseEquipmentData_B
    {
        public int PierceCount { get => _pierceCount; }
        [SerializeField]
        private int _pierceCount = 1;
    }
}