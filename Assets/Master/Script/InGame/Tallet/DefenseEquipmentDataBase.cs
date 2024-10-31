using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
[CreateAssetMenu(menuName = "GameData/DefenseData/TurretDataBase", fileName = "DefebseEquipmentDataBase")]
    public class DefenseEquipmentDataBase : ScriptableObject
    {
        public string Name { get; private set; }
        public string Explanation { get; private set; }
        public int DropChance { get; private set; }
        public List<DefenseEquipmentData_B> DataLevelList { get; private set; }
    }
}
