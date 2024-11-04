using UnityEngine;

namespace DCFrameWork
{
    [CreateAssetMenu(fileName = "NewData", menuName = "ScriptableObject/EquipmentCard")]
    public class EquipmentCardData : ScriptableObject
    {
        public Texture2D EquipmentIcon;
        public string EquipmentName;
        public int EquipmentLevel;
    }
}
