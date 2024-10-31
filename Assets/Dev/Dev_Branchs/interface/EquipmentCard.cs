using UnityEngine;

namespace DCFrameWork
{
    [CreateAssetMenu(fileName = "NewData", menuName = "ScriptableObject/EquipmentCard")]
    public class EquipmentCard : ScriptableObject
    {
        public Texture2D EquipmentIcon;
        public string EquipmentName;
    }
}
