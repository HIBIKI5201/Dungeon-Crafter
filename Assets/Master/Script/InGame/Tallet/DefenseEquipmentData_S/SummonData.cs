using DCFrameWork.DefenseEquipment;
using System;
using UnityEngine;

namespace DCFrameWork
{
    [Serializable, CreateAssetMenu(menuName = "GameData/DefenseData/Summon_Data", fileName = "DefebseEquipmentData")]
    public class SummonData : DefenseEquipmentData_B
    {
        public int MaxCount { get => _maxCount; }
        [SerializeField, Header("召喚可能数")]
        private int _maxCount = 1;
    }
}
