using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public class DefenseEquipmentDataBase : ScriptableObject
    {
        public string Name;
        public string Explanation;
        public int DropChance;
        public List<DefenseEquipmentData_B> DataLevelList;
    }
}
