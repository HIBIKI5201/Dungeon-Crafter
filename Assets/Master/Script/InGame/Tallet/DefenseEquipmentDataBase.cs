using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
[CreateAssetMenu(menuName = "GameData/DefenseData/TurretDataBase", fileName = "DefebseEquipmentDataBase")]
    public class DefenseEquipmentDataBase : ScriptableObject
    {
        public string Name;
        public string Explanation;
        public int DropChance;
        public List<DefenseEquipmentData_B> DataLevelList;
        public List<int> PowerUpRequireItem;
    }
}
