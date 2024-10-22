using UnityEngine;
namespace DCFrameWork.DefenseEquipment
{
    [CreateAssetMenu(menuName = "GameData/DefenseData/Base_Data", fileName = "DefebseEquipmentData")]
    public class DefenseEquipmentData_B : ScriptableObject
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