using DCFrameWork.DefenseEquipment;
using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork
{
    [CreateAssetMenu(menuName = "GameData/DefenseData/TurretDataBase", fileName = "DefebseEquipmentDataBase")]
    public class DefenseEquipmentDataBase : ScriptableObject
    {
        public string EquipmentName;
        public int DropChance;

        public List<DefenseEquipmentData_B> DataLevelList;
    }
}
