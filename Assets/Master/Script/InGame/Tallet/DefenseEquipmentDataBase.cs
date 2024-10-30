using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public class DefenseEquipmentDataBase : ScriptableObject
    {
        public string Name;
        public string Explanation;

        public List<DefenseEquipmentData_B> DataLevelList;
    }
}
