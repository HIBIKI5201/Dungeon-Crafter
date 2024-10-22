using UnityEngine;
namespace DCFrameWork.DefenseEquipment
{
    public class DefenseEquipmentData_B : ScriptableObject
    {
        //情報
        public string EquipmentName;
        public int DropChance;
        //ステータス
        public float Attack;
        public float Rate;
        public float Range;
        public float Critical;
    }
}