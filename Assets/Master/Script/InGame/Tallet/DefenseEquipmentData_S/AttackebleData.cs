using DCFrameWork.DefenseEquipment;
using System;
using UnityEngine;

namespace DCFrameWork
{
    [Serializable, CreateAssetMenu(menuName = "GameData/DefenseData/Attackeble_Data", fileName = "DefebseEquipmentData")]
    public class AttackebleData : DefenseEquipmentData_B
    {
        public float Range;
    }
}
