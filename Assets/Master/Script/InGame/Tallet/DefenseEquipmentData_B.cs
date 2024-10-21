using UnityEngine;
namespace DCFrameWork.DefenseEquipment
{
    public abstract class DefenseEquipmentData_B : ScriptableObject
    {
        //���
        public string EquipmentName;
        public int DropChance;
        //�X�e�[�^�X
        public float Attack;
        public float Rate;
        public float Range;
        public float Critical;
    }
}