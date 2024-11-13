using System;
using UnityEngine;
namespace DCFrameWork.DefenseEquipment
{
    [Serializable, CreateAssetMenu(menuName = "GameData/DefenseData/Common_Data", fileName = "DefebseEquipmentData")]
    public class DefenseEquipmentData_B : ScriptableObject
    {
        public float Attack;
        public float Rate;
        public float Critical;
    }
}